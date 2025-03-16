using Godot;

public partial class Enemy : CharacterBody2D
{
    [Export] public string ElementType { get; set; } = "Fire";
    [Export] public float Speed = 50f;
    [Export] public float Health { get; set; } = 100f;

    private Player player;
    private float attackCooldown = 2f;
    private float attackTimer = 0f;

    public override void _Ready()
    {
        AddToGroup("enemies"); // Add this enemy to the "enemies" group
        player = GetTree().Root.GetNode<Player>("Game/Player");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (player == null) return;
        Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
        Velocity = direction * Speed;
        MoveAndSlide();  // figure out how to add speed to this

        attackTimer -= (float)delta;
        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = attackCooldown;
        }

        if (Health <= 0)
        {
            EmitSignal(nameof(EnemyDied)); // Emit signal when enemy dies
            QueueFree();
        }
    }

    private void Attack()
    {
        var projectileScene = GD.Load<PackedScene>("res://Scenes/Projectile.tscn");
        var projectile = projectileScene.Instantiate<Projectile>();
        projectile.Direction = (player.GlobalPosition - GlobalPosition).Normalized();
        projectile.GlobalPosition = GlobalPosition;
        GetParent().AddChild(projectile);
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Die();
        }
    }
    [Signal]
    public delegate void EnemyDiedEventHandler();
    private void Die()
    {
        //var sfx = GetNode<AudioStreamPlayer>("DeathSFX"); // Play death sound
        //sfx.Play();
        player.Stats.GainExperience(10);
        player.Stats.KillEnemy(ElementType);
        QueueFree();
    }

    private void OnBodyEntered(Node2D body)
    {
        //GD.Print("Body entered: " + body.Name);
        //if (body is Missile)
        //{

        //    GD.Print("Enemy hit by missile");
        //    body.QueueFree();
        //}
    }

    private void OnAreaEntered(Area2D area)
    {
        GD.Print("Body entered: " + area.Name);
        if (area is Missile)
        {

            GD.Print("Enemy hit by missile");
            area.QueueFree();
        }
    }
}