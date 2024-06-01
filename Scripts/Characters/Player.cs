using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export]
	public float Sensitivity = 0.1f; // Mouse sensitivity
	private AnimationPlayer _animationPlayer;
	private bool _canInteract = false;
	private Interactable _currentInteractable = null;
	private Node3D _twistPivotNode;
	private PlayerHUD _playerHUD;


	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	private AnimationPlayer _animationPlayer2;


	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();



	public override void _Ready()
	{
		// GEt Player HUD
		GD.Print($"Player HUD: {this.GetParent().Name}");
		foreach (Node child in this.GetParent().GetChildren())
		{
			GD.Print($"Child Node Name: {child.Name}");
		}
		_playerHUD = GetParent().GetNode<PlayerHUD>("PlayerHUD");

		if (_playerHUD != null)
		{
			GD.Print("PlayerHUD found!");
		}
		else
		{
			GD.Print("PlayerHUD not found!");
		}       // Animations
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

		if (@event is InputEventMouseMotion mouseMotion)
		{
			// Horizontal rotation (yaw) on TwistPivotNode
			RotateY(Mathf.DegToRad(-mouseMotion.Relative.X * Sensitivity));

			// Vertical rotation (pitch) on PitchPivotNode
			float newPitchRotation = _twistPivotNode.RotationDegrees.X - (mouseMotion.Relative.Y * Sensitivity);
			newPitchRotation = Mathf.Clamp(newPitchRotation, -90, 90);
			_twistPivotNode.RotationDegrees = new Vector3(newPitchRotation, _twistPivotNode.RotationDegrees.Y, _twistPivotNode.RotationDegrees.Z);
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
			_canInteract = true;
			// _currentInteractable = interactable;
			if (body.GetParent() != null)
			{
				Node parent = body.GetParent();
				string parentName = parent.Name;

				// GD.Print($"Parent Name: {parentName} Type: {parent.GetParent().Name} {body.GetType().FullName}");
				if (parentName.Contains("barrel", StringComparison.OrdinalIgnoreCase))
				{
					HandleBarrel();

				}
				if (parentName.Contains("espresso", StringComparison.OrdinalIgnoreCase))
				{
					HandleEspresso();
				}
			}
		}
	}

	private void HandleBarrel()
	{
		GD.Print("Barrel handle");
		_playerHUD.ShowPrompt("Press E to interact with the barrel.");
	}

	private void HandleEspresso()
	{
		GD.Print("espresso handle");
		_playerHUD.ShowPrompt("Press E to interact with the espresso.");
	}

	private void HandleMilk()
	{
		GD.Print("milk handle");
		_playerHUD.ShowPrompt("Press E to interact with the milk.");
	}

	private void HandleSyrup()
	{
		GD.Print("syrupey handle");
		_playerHUD.ShowPrompt("Press E to interact with the syrup.");
	}



	public void _on_Area3D_body_exited(Node body)
	{
		if (body is Interactable interactable && interactable == _currentInteractable)
		{
			GD.Print("exit area in Player Class.");
			_playerHUD.HidePrompt();
			_canInteract = false;
			_currentInteractable = null;
		}
	}
}





