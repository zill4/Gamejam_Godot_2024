using Godot;
using System;

public partial class CoffeeCup : Interactable
{
    public override void Interact()
    {
        GD.Print("Interacting with the Coffee Cup.");
    }
}
