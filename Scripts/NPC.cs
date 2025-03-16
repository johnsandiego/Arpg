using Godot;

public partial class NPC : CharacterBody2D
{
    private Vector2[] path = new Vector2[] { new Vector2(100, 100), new Vector2(200, 200) }; // Work, Sleep positions
    private int pathIndex = 0;
    [Export] public float Speed = 50f;
    private float timer = 0f;
    private float waitTime = 5f; // Time spent at each location

    public override void _PhysicsProcess(double delta)
    {
        timer -= (float)delta;
        if (timer <= 0)
        {
            Vector2 target = path[pathIndex];
            Vector2 direction = (target - GlobalPosition).Normalized();
            MoveAndSlide(); // figure out how to add speed to this

            if (GlobalPosition.DistanceTo(target) < 10)
            {
                timer = waitTime;
                pathIndex = (pathIndex + 1) % path.Length;
            }
        }
    }
}