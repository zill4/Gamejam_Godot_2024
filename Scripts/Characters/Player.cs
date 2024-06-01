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
	

	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	
	
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    


	public override void _Ready()
	{
		// Animations
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
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

	    // Change these methods to public to allow access from Interactable
    public void _on_Area3D_body_entered(Node body)
    {
		// GD.Print("Not crazy entered area. :3");
        if (body is Area3D interactable)
        {
			 GD.Print("enter area.");
            _canInteract = true;
            // _currentInteractable = interactable;
        }
    }

    public void _on_Area3D_body_exited(Node body)
    {
        if (body is Interactable interactable && interactable == _currentInteractable)
        {
			 GD.Print("exit area.");
            _canInteract = false;
            _currentInteractable = null;
        }
    }
}
