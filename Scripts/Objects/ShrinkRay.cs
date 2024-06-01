using Godot;
using System;

public partial class ShrinkRay : Node
{
    public void Interact()
    {
        GD.Print("Interacting with the Espresso Machine.");
        // Add custom logic for Espresso Machine interaction
    }

    ShrinkRay()
    {
        GD.Print($"Espresso Machine Better be created. {this.Name} {this.GetType()} {this.Owner} {this.GetParent()}");
    }
}
