using Godot;
using System;

public abstract partial class Interactable : Node3D
{
    // Called when the node enters the scene tree for the first time.
    // Abstract method to be implemented by derived classes

    public abstract void Interact();
}
