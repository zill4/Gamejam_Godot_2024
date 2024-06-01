using Godot;

public partial class Interactable : Area3D
{
    public override void _Ready()
    {
        // Connecting signals with C# callable references
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        Connect("body_exited", new Callable(this, nameof(OnBodyExited)));
    }

    private void OnBodyEntered(Node body)
    {
        GD.Print("MEOW entered interact class. :3");
        if (body.Name == "Player")
        {
            GD.Print("entered interact class. :3");
            Player player = (Player)body;
            player.CallDeferred(nameof(Player._on_Area3D_body_entered), this);
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            GD.Print("exited interact class. :3");
            Player player = (Player)body;
            player.CallDeferred(nameof(Player._on_Area3D_body_exited), this);
        }
    }

    public void PerformAction()
    {
        GD.Print("Action performed!");
        // Implement your interaction logic here
    }
}
