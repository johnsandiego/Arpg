using Godot;

public partial class Projectile : Area2D
{
    [Export] public Vector2 Direction { get; set; } = Vector2.Right;
    [Export] public float Speed = 200f;

    public override void _PhysicsProcess(double delta)
    {
        Position += Direction * Speed * (float)delta;
    }

    private void OnProjectileBodyEntered(Node body)
    {
        if (body is Player player)
        {
            player.TakeDamage(10);
            QueueFree();
        }
    }
}