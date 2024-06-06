using Godot;
using System;

public partial class InteractionTrigger : Area3D
{
	// private PlayerHUD _playerHUD;
    private Node _currentInteractable;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{        
        // Interaction Trigger setup
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
	}

    private void OnBodyEntered(Node body)
    {
        if (body is Player player  && _currentInteractable == null)
        {
            _currentInteractable = player;
            player.CallDeferred(nameof(Player._on_Area3D_body_entered), this);
        }
        else
        {
            GD.Print($"interacted with {body.Name}");
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body is Player player && _currentInteractable == player)
        {
            GD.Print("exited interact class. :3");
            _currentInteractable = null;
            player.CallDeferred(nameof(Player._on_Area3D_body_exited), this);
        }
    }

}
