using Godot;
using System;

public partial class InteractionTrigger : Area3D
{
	private PlayerHUD _playerHUD;
    private Node _currentInteractable;
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
        if (body.HasMethod("Interact"))
        {
            GD.Print($" triggered by {body.Name}");
            // Add your custom logic here
			_playerHUD.ShowMessage($"Press 'E' to interact with  MEOW");
            _currentInteractable = body;
        }
    }

    private void OnBodyExited(Node body)
    {
        if (_currentInteractable == body)
        {
            GD.Print($"{body.Name} exited  trigger area");
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
            _currentInteractable.Call("Interact");
        }
    }
}
