using Godot;
using System;

public partial class InteractableObject : Area3D
{
	[Export]
	public string ObjectName = "Object Name";
	private PlayerHUD _playerHUD;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{        
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
		_playerHUD = GetNode<PlayerHUD>("/root/Game/PlayerHud"); // Adjust the path as needed

	}

    private void OnBodyEntered(Node body)
    {
        if (body is Player) // Assuming Player is the name of your player class
        {
            GD.Print($"{ObjectName} triggered by {body.Name}");
            // Add your custom logic here
			_playerHUD.ShowMessage($"Press 'E' to interact with {ObjectName} MEOW");
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is Player)
        {
            GD.Print($"{body.Name} exited {ObjectName}'s trigger area");
            // Add your custom logic here
			_playerHUD.ShowMessage($"Go find a coffee machine pls :3");
        }
    }
}
