using Godot;
using System;
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



	public override void _Ready()
	{
		// popup bubble
		_popupBubble = GetNode<PopupBubble>("PopupBubble");
		_popupBubble.Hide();
		// GEt Player HUD
		GD.Print($"Player HUD: {this.GetParent().Name}");
		GD.Print($"Parent Path: {this.GetParent().GetPath()}");
		foreach (Node child in this.GetParent().GetChildren())
		{
			GD.Print($"Child Node Name: {child.Name}");
		}
		_playerHUD = GetParent().GetNode<PlayerHUD>("PlayerHud");

		if (_playerHUD != null)
		{
			GD.Print("PlayerHUD found!");
		}
		else
		{
			GD.Print("PlayerHUD not found!");
		}
		// Animations
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer2");
		_animationPlayer.Play("idle");

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

	public void Interact(string interactableName)
	{
		switch (interactableName)
		{
			case "barrel":
				HandleBarrel();
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
			default:
				break;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

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


		Velocity = velocity;
		MoveAndSlide();
	}

	private void _on_interaction_trigger_body_entered(Node3D body)
	{
		if (body.Name == "portal")
		{
			GD.Print("please work. :3");
		}
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
				Node parent = body.GetParent();
				string parentName = parent.Name;

				GD.Print($"Parent Name: {parentName} Type: {parent.GetParent().Name} {body.GetType().FullName}");
				if (parentName.Contains("barrel", StringComparison.OrdinalIgnoreCase))
				{
					_canInteract = true;
					_interactableName = "barrel";
					_playerHUD.ShowPrompt("Press E to interact with the barrel.");

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
			GD.Print("exit area in Player Class.");
			_currentInteractable = null;
		}
	}
	private void HandleBarrel()
	{

		GD.Print("Barrel handle");
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





