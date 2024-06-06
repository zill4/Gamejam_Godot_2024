using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class Player : CharacterBody3D
{
	[Export]
	public float Sensitivity = 0.1f; // Mouse sensitivity
	private AnimationPlayer _animationPlayer;
	private bool _canInteract = false;
	private string _interactableName = "";
	private Interactable _currentInteractable = null;
	private Node3D _twistPivotNode;
	private PlayerHUD _playerHUD;
	public PopupBubble _popupBubble;
    private const float ShrinkScale = 0.15f; // Adjust this value to make the sprites smaller or larger

	private bool _canDeleteCustomer = false;
	private customer _currentCustomer = null;
	private Node3D sharpieNode;
	private Vector3 sharpieOriginalPosition;
    private Basis sharpieOriginalRotation;

	// ingredients
	private Dictionary<string, int> _ingredients = new Dictionary<string, int>
		{
			{ "milk", 0 },
			{ "hazelnut", 0 },
			{ "mocha", 0 },
			{ "espresso", 0 },
			{ "cup", 0 }
		};

	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	private AnimationPlayer _animationPlayer2;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public List<string> GetIngredientsList()
	{
		List<string> ingredientsList = new List<string>();

		foreach (var ingredient in _ingredients)
		{
			if (ingredient.Value >= 1)
			{
				ingredientsList.Add(ingredient.Key);
			}
		}

		return ingredientsList;
	}

	public override void _Ready()
	{
		// sharpie
		        sharpieNode = GetNode<Node3D>("sharpie2");

        if (sharpieNode != null)
        {
            sharpieOriginalPosition = sharpieNode.Transform.Origin;
            sharpieOriginalRotation = sharpieNode.Transform.Basis;
        }

		// popup bubble
		_popupBubble = GetNode<PopupBubble>("PopupBubble");
		_popupBubble.Hide();

		// GEt Player HUD
		_playerHUD = GetParent().GetNode<PlayerHUD>("PlayerHud");

		// Animations
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.Play("idle");
		// _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer2");

		_twistPivotNode = GetNode<Node3D>("TwistPivot");
		// Hide the mouse cursor and capture it.
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Input(InputEvent @event)
	{
		// assuming this is necessary for the override
		base._Input(@event);

		if (@event is InputEventMouseMotion mouseMotion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			// Horizontal rotation (yaw) on TwistPivotNode
			RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * Sensitivity));

			// Vertical rotation (pitch) on PitchPivotNode
			float newPitchRotation = _twistPivotNode.RotationDegrees.X - (mouseMotion.Relative.Y * Sensitivity);
			newPitchRotation = Mathf.Clamp(newPitchRotation, -90, 90);
			_twistPivotNode.RotationDegrees = new Vector3(newPitchRotation, _twistPivotNode.RotationDegrees.Y, _twistPivotNode.RotationDegrees.Z);
		}

		if (@event.IsActionPressed("interact") && _canInteract)
		{
			Interact(_interactableName);
		}

		if (@event.IsActionPressed("attack"))
		{
			AttackAction();
		}

	}

	public void ShowPopupBubble()
	{
		_popupBubble.Clear(); // Clear any previous content
		_popupBubble.SetSprite(0, (Texture2D)GD.Load("res://Assets/sprites/ingredient icons/milk.png"));
		_popupBubble.SetSprite(1, (Texture2D)GD.Load("res://Assets/sprites/ingredient icons/hazelnut.png"));
		_popupBubble.SetSprite(2, (Texture2D)GD.Load("res://Assets/sprites/ingredient icons/beans.png"));
		_popupBubble.SetSprite(3, (Texture2D)GD.Load("res://Assets/sprites/ingredient icons/steamed.png"));
		_popupBubble.ShowBubble();
	}

	public void HidePopupBubble()
	{
		_popupBubble.HideBubble();
	}

	public void HandleCustomer()
	{
		if (_canDeleteCustomer && _currentCustomer != null)
		{
			if (_currentCustomer.getOrder().Drink.CheckIngredients(GetIngredientsList()))
			{
				_currentCustomer.SetStatus(OrderStatus.COMPLETED);
				_playerHUD.IncreaseSuccessScore();
				_playerHUD.ShowPrompt("Order Completed!");
			}
			else
			{
				_currentCustomer.SetStatus(OrderStatus.FAILED);
				_playerHUD.IncreaseFailedScore();
				_playerHUD.ShowPrompt("Order Failed!");
			}
			_currentCustomer.DeleteCustomer();
			_currentCustomer = null;
			_canDeleteCustomer = false;
			ClearIngredients();
			_popupBubble.Clear(); // Clear any previous content
		}
	}

	public void HandlePortal()
	{
		var childNode = GetNode<Node3D>("sharpie2");
		if (this.Scale.X == ShrinkScale)
		{
			this.Scale = new Vector3(.5f, .5f, .5f);
			childNode.Scale = new Vector3(5.0f, 5.0f, 5.0f);
		}
		else
		{
			this.Scale = new Vector3(ShrinkScale, ShrinkScale, ShrinkScale);
			childNode.Scale = new Vector3(15.0f, 15.0f, 15.0f);
		}
	}

	public void AttackAction()
	{
        if (sharpieNode != null)
        {
			// sharpieOriginalPosition = sharpieNode.Transform.Origin;
            // sharpieOriginalRotation = sharpieNode.GlobalTransform.Basis;
            PerformAttack();
        }	
	}
    private void PerformAttack()
    {
        // Rotate and move forward
        RotateAndMove(sharpieNode, new Vector3(0, 0, -0.5f), new Vector3(90, 0, 0), 0.1f);

        // Wait
        // yield return new WaitForSeconds(0.1f);

        // Move back
        RotateAndMove(sharpieNode, new Vector3(0, 0, 0.5f), new Vector3(-90, 0, 0), 0.1f);

        // Wait
        // yield return new WaitForSeconds(0.1f);

        // Restore original position and rotation
        sharpieNode.Transform = new Transform3D(sharpieOriginalRotation, sharpieOriginalPosition );
    }

	private IEnumerator RotateAndMove(Node3D node, Vector3 moveBy, Vector3 rotateBy, float duration)
    {
        Vector3 targetPosition = node.Transform.Origin + moveBy;
 		Quaternion targetRotationQuat = new Basis().Rotated(Vector3.Right, rotateBy.X)
                                                   .Rotated(Vector3.Up, rotateBy.Y)
                                                   .Rotated(Vector3.Forward, rotateBy.Z)
                                                   .GetRotationQuaternion();

		Basis targetRotation = new Basis(targetRotationQuat) * node.Transform.Basis;
        double elapsed = 0;
        while (elapsed < duration)
        {
            double t = elapsed / duration;

    			node.Transform = new Transform3D(
                new Basis(node.Transform.Basis.GetRotationQuaternion().Slerp(targetRotation.GetRotationQuaternion(), (float)t)),
                node.Transform.Origin.Lerp(targetPosition, (float)t));

            elapsed += GetProcessDeltaTime();
            yield return null;
        }

        node.Transform = new Transform3D(targetRotation, targetPosition);
    }

	public void Interact(string interactableName)
	{
		switch (interactableName)
		{
			case "trash":
				Handletrash();
				break;
			case "espresso":
				HandleEspresso();
				break;
			case "milk":
				HandleMilk();
				break;
			case "hazelnut":
				HandleSyrup(interactableName);
				break;
			case "mocha":
				HandleSyrup(interactableName);
				break;
			case "cup":
				HandleCup();
				break;
			case "customer":
				HandleCustomer();
				break;
			case "portal":
				HandlePortal();
				break;
			default:
				break;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y -= gravity * (float)delta;
			_animationPlayer.Pause();
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		{
			_animationPlayer.Play("jump");
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}
		// Show cursor when the game is paused.
		if (Input.IsActionJustPressed("ui_cancel") && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
		else if (Input.IsActionJustPressed("ui_cancel") && Input.MouseMode == Input.MouseModeEnum.Visible)
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}


		//    if (_popupBubble.Visible)
		//     {
		//         Vector3 bubblePosition = GlobalTransform.Origin + new Vector3(0, 2, 0); // Adjust the offset as needed
		//         Transform3D currentTransform = _popupBubble.GlobalTransform;
		//         currentTransform.Origin = bubblePosition;
		//         _popupBubble.GlobalTransform = currentTransform;
		//     }
		if (Velocity.Length() > 0.1  && IsOnFloor())
		{
			_animationPlayer.Play("waddle");
			_animationPlayer.SpeedScale = .50f;
		}
		else
		{
			_animationPlayer.SpeedScale = 1.0f;
			_animationPlayer.Play("idle");
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	// Change these methods to public to allow access from Interactable
	public void _on_Area3D_body_entered(Node body)
	{
		// GD.Print("Not crazy entered area. :3");
		if (body is Area3D interactable)
		{
			// _currentInteractable = interactable;
			if (body.GetParent() != null)
			{
				GD.Print($"Entered Player Area, {body.GetParent().Name} {body.Name}");
				Node parent = body.GetParent();
				string parentName = parent.Name;

				// GD.Print($"Parent Name: {parentName} Type: {parent.GetParent().Name} {body.GetType().FullName}");
				if (parentName.Contains("trash", StringComparison.OrdinalIgnoreCase))
				{
					_canInteract = true;
					_interactableName = "trash";
					_playerHUD.ShowPrompt("Press E to interact with the trash.");

				}
				if (parentName.Contains("espresso", StringComparison.OrdinalIgnoreCase))
				{
					_canInteract = true;
					_interactableName = "espresso";
					_playerHUD.ShowPrompt("Press E to interact with the espresso.");
				}
				if (parentName.Contains("milk", StringComparison.OrdinalIgnoreCase))
				{
					_canInteract = true;
					_interactableName = "milk";
					_playerHUD.ShowPrompt("Press E to interact with the milk.");
				}
				if (parentName.Contains("syrup", StringComparison.OrdinalIgnoreCase))
				{
					_canInteract = true;
					if (parentName.Contains("hazelnut", StringComparison.OrdinalIgnoreCase))
					{
						_interactableName = "hazelnut";
						_playerHUD.ShowPrompt("Press E to interact with the Hazelnut syrup.");
					}
					else if (parentName.Contains("mocha", StringComparison.OrdinalIgnoreCase))
					{
						_interactableName = "mocha";
						_playerHUD.ShowPrompt("Press E to interact with the Mocha syrup.");
					}

				}
				if (parentName.Contains("cup", StringComparison.OrdinalIgnoreCase))
				{
					_canInteract = true;
					_interactableName = "cup";
					_playerHUD.ShowPrompt("Press E to interact with the cup.");
				}
				if (parentName.Contains("customer", StringComparison.OrdinalIgnoreCase))
				{
					_canInteract = true;
					_interactableName = "customer";
					_playerHUD.ShowPrompt("Press E to give drink.");
					_currentCustomer = parent as customer;
					_canDeleteCustomer = true;
				}
				if (parentName.Contains("portal", StringComparison.OrdinalIgnoreCase))
				{
					_canInteract = true;
					_interactableName = "portal";
					_playerHUD.ShowPrompt("Press E to enter the portal.");
				}
			}
		}
	}

	public void _on_Area3D_body_exited(Node body)
	{
		// Clear Interactable
		_playerHUD.HidePrompt();
		_canInteract = false;
		_interactableName = "";
		if (body is Interactable interactable && interactable == _currentInteractable)
		{
			// GD.Print("exit area in Player Class.");
			_currentInteractable = null;
		}

		if (body is Area3D)
		{
			if (body.GetParent() != null)
			{
				Node parent = body.GetParent();
				string parentName = parent.Name;
				if (parentName.Contains("customer", StringComparison.OrdinalIgnoreCase))
				{
					_canDeleteCustomer = false;
					_currentCustomer = null;
					GD.Print("Exited collision with customer.");
				}
			}
		}
	}
	private void Handletrash()
	{
		ClearIngredients();
		_popupBubble.Clear(); // Clear any previous content
	}

	private void HandleCup()
	{
		if (_ingredients["cup"] == 0)
		{
			AddIngredient("cup");
			_popupBubble.ShowBubble();
			_popupBubble.SetSprite(3, (Texture2D)GD.Load("res://Assets/sprites/ingredient icons/steamed.png"));
		}
		else
		{
			GD.Print("cup is full");
		}
	}

	private void HandleEspresso()
	{
		GD.Print("espresso handle");
		if (_ingredients["espresso"] == 0)
		{
			AddIngredient("espresso");
			_popupBubble.ShowBubble();
			_popupBubble.SetSprite(2, (Texture2D)GD.Load("res://Assets/sprites/ingredient icons/beans.png"));
		}
		else
		{
			GD.Print("espresso is full");
		}
	}

	private void HandleMilk()
	{
		GD.Print("milk handle");
		if (_ingredients["milk"] == 0)
		{
			AddIngredient("milk");
			_popupBubble.ShowBubble();
			_popupBubble.SetSprite(0, (Texture2D)GD.Load("res://Assets/sprites/ingredient icons/milk.png"));
		}
		else
		{
			GD.Print("milk is full");
		}
	}

	private void HandleSyrup(string syrupType)
	{
		GD.Print("syrupey handle");

		if (_ingredients["hazelnut"] == 0 && _ingredients["mocha"] == 0)
		{
			_popupBubble.ShowBubble();
			switch (syrupType)
			{
				case "hazelnut":
					_popupBubble.SetSprite(1, (Texture2D)GD.Load("res://Assets/sprites/ingredient icons/hazelnut.png"));
					break;
				case "mocha":
					_popupBubble.SetSprite(1, (Texture2D)GD.Load("res://Assets/sprites/ingredient icons/mocha.png"));
					break;
				default:
					break;
			}
			AddIngredient(syrupType);
		}
		else
		{
			GD.Print("Syrup is full");
		}
	}

	public void ClearIngredients()
	{
		foreach (var ingredient in _ingredients)
		{
			_ingredients[ingredient.Key] = 0;
		}
	}

	public void AddIngredient(string ingredient)
	{
		if (_ingredients.ContainsKey(ingredient))
		{
			_ingredients[ingredient]++;
		}
	}

}





