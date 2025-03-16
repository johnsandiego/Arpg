using Godot;

public partial class Player : CharacterBody2D
{
    //movement
    [Export] private float Speed = 300.0f;

    [Export] public float WalkSpeed = 100f;
    [Export] public float RunSpeed = 200f;
    [Export] public float DashSpeed = 400f;
    [Export] public float DashDuration = 0.2f;

    private Vector2 velocity = Vector2.Zero;
    private bool isDashing = false;
    private float dashTimer = 0f;

    [Export] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; private set; }
    public int Level { get; private set; } = 1;
    private float experience = 0f;
    private float expToNextLevel = 100f;
    private ProgressBar healthBar;

    //stats
    public PlayerStats Stats { get; private set; }
    private AttackManager attackManager;
    private AnimatedSprite2D sprite;

    public override void _Ready()
    {
        Stats = new PlayerStats(); // (PlayerStats)GetNode("/root/SaveGame").Call("load");
        GetNode<Camera2D>("Camera2D").Offset = new Vector2(0, -50); // Offset above player
        attackManager = GetNode<AttackManager>("AttackManager");
        sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        healthBar = GetNode<ProgressBar>("UI/HealthBar");


    }

    public override void _PhysicsProcess(double delta)
    {
        Movement((float)delta);

    }

    public void Movement(float delta)
    {
        if (isDashing)
        {
            dashTimer -= (float)delta;
            if (dashTimer <= 0) isDashing = false;
            else
            {
                MoveAndSlide();
                return;
            }
        }

        float speed = Input.IsActionPressed("run") ? RunSpeed : WalkSpeed;
        Vector2 input = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down").Normalized();
        velocity = input * speed;

        if (Input.IsActionJustPressed("dash") && input != Vector2.Zero)
        {
            isDashing = true;
            dashTimer = DashDuration;
            velocity = input * DashSpeed;
        }

        if (input != Vector2.Zero)
        {
            sprite.Play("run");
            sprite.FlipH = input.X < 0;
        }
        else
        {
            sprite.Play("idle");
        }
        Velocity = velocity;
        MoveAndSlide();
    }

    public override void _Input(InputEvent @event)
    {
        HandleAttacks();
    }

    private void HandleAttacks()
    {
        if (Input.IsActionJustPressed("attack_1")) attackManager.ExecuteAttack(AttackType.BloodSlash);
        if (Input.IsActionJustPressed("attack_2")) attackManager.ExecuteAttack(AttackType.DarkWave);
        if (Input.IsActionJustPressed("attack_3")) attackManager.ExecuteAttack(AttackType.BatSwarm);
        // Add more attack inputs as needed
    }

    public void TakeDamage(int amount)
    {
        Stats.CurrentHP -= amount;
        if (Stats.CurrentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GD.Print("Player Died");
        // Add game over logic here (e.g., load menu or restart)
        GetTree().ReloadCurrentScene();

    }


    public void GainExperience(float amount)
    {
        experience += amount;
        while (experience >= expToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        experience -= expToNextLevel;
        Level++;
        expToNextLevel *= 1.5f;
        MaxHealth += 20f;
        CurrentHealth = MaxHealth;
        RunSpeed += 20f;
        GD.Print($"Level Up! Now Level {Level}");
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        UpdateHealthBar();
        if (CurrentHealth <= 0)
        {
            GD.Print("Game Over!");
            // Add game over logic here
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.Value = (CurrentHealth / MaxHealth) * 100;
    }
}