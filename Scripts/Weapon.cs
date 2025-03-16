// Weapon.cs
using Godot;

public partial class Weapon : Node2D
{
    [Export] private float orbitRadius = 50f;    // Distance from player
    [Export] private float swingSpeed = 10f;     // Speed of swing animation
    [Export] private float swingAngle = 90f;     // Max angle of swing in degrees
    [Export] private float followSpeed = 15f;    // How quickly it follows mouse

    private bool isSwinging = false;             // Swing state
    private float swingProgress = 0f;            // Swing animation progress (0 to 1)
    private Vector2 swingTarget;                 // Target direction for swing
    private Vector2 targetPosition;              // Desired position based on mouse
    [Export]
    public Player player;                     // Reference to player node
    public override void _Ready()
    {
        Visible = true;
        // Initialize position
        targetPosition = new Vector2(orbitRadius, 0);
        Position = targetPosition;
        if (player == null)
        {
            GD.PrintErr("Player node not found!");
        }
    }

    public override void _Process(double delta)
    {
        float fDelta = (float)delta;

        if (!isSwinging)
        {
            // Calculate target position based on mouse
            Vector2 mousePos = GetGlobalMousePosition();
            Vector2 playerPos = player.GlobalPosition;
            Vector2 direction = (mousePos - playerPos).Normalized();
            targetPosition = direction * orbitRadius;

            // Smoothly move towards target position
            Position = Position.Lerp(targetPosition, followSpeed * fDelta);

            // Face outward from player
            Rotation = direction.Angle() + Mathf.Pi / 2;
        }
        else
        {
            // Swing animation
            swingProgress += swingSpeed * fDelta;

            // Calculate swing rotation
            float swingRotation = Mathf.Lerp(
                0f,
                swingAngle * Mathf.DegToRad(1f),
                Mathf.Sin(swingProgress * Mathf.Pi)
            );

            // Apply swing rotation towards target
            Vector2 directionToTarget = (swingTarget - GlobalPosition).Normalized();
            Rotation = directionToTarget.Angle() + swingRotation;

            if (swingProgress >= 1f)
            {
                isSwinging = false;
                swingProgress = 0f;
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            //GD.Print("Firing missile");

            var missile = GD.Load<PackedScene>("res://Scenes/missile.tscn").Instantiate<Missile>();
            //GD.Print("Missile: " + missile);
            var spawnPosition = GetNode<Node2D>("SpawnPosition");
            //GD.Print("SpawnPosition: " + spawnPosition);

            Vector2 mousePos = GetGlobalMousePosition();
            Vector2 spawnPos = spawnPosition.GlobalPosition;

            // Calculate direction from spawn to mouse
            Vector2 direction = (mousePos - spawnPos).Normalized();
            // Set rotation based on direction
            missile.Rotation = direction.Angle();

            missile.Position = spawnPos;
            GetTree().Root.AddChild(missile);
        }
    }
}