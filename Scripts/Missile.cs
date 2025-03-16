// Projectile.cs
using Godot;

public partial class Missile : Area2D
{
    [Export] public float Speed = 200f;
    [Export] public float MaxSpeed = 400f;
    [Export] public float SteeringForce = 50f;
    private Vector2 _targetPosition;
    private Vector2 _velocity;

    public override void _Ready()
    {
        // Set initial target as mouse position
        _targetPosition = GetGlobalMousePosition();
        _velocity = Transform.X * Speed; // Start moving forward
    }

    public override void _PhysicsProcess(double delta)
    {
        // Update target position (could be mouse or a specific target)
        _targetPosition = GetGlobalMousePosition();

        // Calculate desired direction
        Vector2 desired = (_targetPosition - GlobalPosition).Normalized() * MaxSpeed;
        // Steering = desired - current
        Vector2 steering = (desired - _velocity).LimitLength(SteeringForce);
        // Apply steering
        _velocity += steering * (float)delta;
        _velocity = _velocity.LimitLength(MaxSpeed);

        // Move projectile
        Position += _velocity * (float)delta;
        // Rotate to face movement direction
        Rotation = _velocity.Angle();

        // Optional: Destroy when off-screen
        if (!GetViewportRect().HasPoint(GlobalPosition))
        {
            QueueFree();
        }
    }

    // Optional: Handle collision
    private void OnBodyEntered(Node body)
    {
        QueueFree(); // Destroy on impact
        // Add damage logic here if needed
    }
}