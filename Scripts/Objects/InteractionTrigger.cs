using Godot;
using System;

public partial class InteractionTrigger : Area3D
{
	[Export]
	public string ObjectName = "Object Name";
	private PlayerHUD _playerHUD;
    private Interactable _currentInteractable;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{        
        // Interaction Trigger setup
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("body_exited", new Callable(this, nameof(OnBodyExited)));

        // Attach to HUD
		_playerHUD = GetNode<PlayerHUD>("/root/Game/PlayerHud"); 

	}

    private void OnBodyEntered(Node body)
    {
        if (body is Interactable interactable) // Assuming Player is the name of your player class
        {
            GD.Print($"{ObjectName} triggered by {body.Name}");
            // Add your custom logic here
			_playerHUD.ShowMessage($"Press 'E' to interact with {ObjectName} MEOW");
            _currentInteractable = interactable;
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is Interactable)
        {
            GD.Print($"{body.Name} exited {ObjectName}'s trigger area");
            // Add your custom logic here
			_playerHUD.ShowMessage($"Go find a coffee machine pls :3");
            _currentInteractable = null;
        }
    }

    public override void _Input(InputEvent @event)
    {
        // Check if the player pressed the 'E' key
        if (@event is InputEventKey inputEventKey && inputEventKey.Pressed && inputEventKey.Keycode == Key.E)
        {
            _currentInteractable?.Interact();
        }
    }
}
