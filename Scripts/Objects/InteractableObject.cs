using Godot;
using System;

public partial class InteractableObject : Area3D
{
	[Export]
	public string ObjectName = "Object Name";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{        
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
	}

    private void OnBodyEntered(Node body)
    {
        if (body is Player) // Assuming Player is the name of your player class
        {
            GD.Print($"{ObjectName} triggered by {body.Name}");
            // Add your custom logic here
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is Player)
        {
            GD.Print($"{body.Name} exited {ObjectName}'s trigger area");
            // Add your custom logic here
        }
    }
}
