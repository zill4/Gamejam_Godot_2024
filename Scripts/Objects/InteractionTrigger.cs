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
        // GD.Print("MEOW entered interact class. :3");
        if (body.GetType() == typeof(Player))
        {
            GD.Print("entered interact class. :3");
            Player player = (Player)body;
            player.CallDeferred(nameof(Player._on_Area3D_body_entered), this);
        }
    }

    private void OnBodyExited(Node body)
    {
        //  GD.Print($"MEOW entered interact class. {body.Name} {body.GetType()} {body.Owner} {body.GetParent()} {body.GetPath()} {body.GetGroups()}");
        if (body.IsInGroup("Player"))
        {
            GD.Print("exited interact class. :3");
            Player player = (Player)body;
            player.CallDeferred(nameof(Player._on_Area3D_body_exited), this);
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
