using Godot;
using System;

public partial class customer : CharacterBody3D
{
    private Vector3 _targetPosition;
    public float Speed = 5.0f;
    private const float TargetThreshold = 0.1f; // Adjust this value as needed

    public override void _PhysicsProcess(double delta)
    {
        Vector3 direction = (_targetPosition - GlobalTransform.Origin).Normalized();
        
        if (GlobalTransform.Origin.DistanceTo(_targetPosition) < TargetThreshold)
        {
            // NPC has reached the target
            Velocity = Vector3.Zero;
        }
        else
        {
            // NPC is still moving towards the target
            Velocity = direction * Speed;
        }
        
        MoveAndSlide();
    }

    public void SetTargetPosition(Vector3 position)
    {
        _targetPosition = position;
    }
}
