// AttackManager.cs
using Godot;
using System.Collections.Generic;

public enum AttackType
{
    BloodSlash, DarkWave, BatSwarm, ShadowStep, VampireBite,
    SoulReap, NightMist, DeathSpike, BloodNova, GrimPulse
}

public partial class AttackManager : Node2D
{
    [Export] private PackedScene bloodSlashEffect;
    [Export] private PackedScene darkWaveEffect;

    private Dictionary<AttackType, Ability> abilities;
    private List<Ability> activeCombo = new List<Ability>();
    private float comboTimer = 0f;
    private const float COMBO_WINDOW = 1.5f; 
    private Player player;

    private GameManager gameManager;

    public override void _Ready()
    {
        player = GetParent<Player>();
        gameManager = GetTree().Root.GetNode<GameManager>("Main");
        GD.Print(gameManager.Name);
        InitializeAbilities();
    }

    public override void _Process(double delta)
    {
        var deltaf = (float)delta;
        if (comboTimer > 0)
        {
            comboTimer -= deltaf;
            if (comboTimer <= 0)
            {
                ExecuteCombo();
                activeCombo.Clear();
            }
        }
    }

    private void InitializeAbilities()
    {
        abilities = new Dictionary<AttackType, Ability>
        {
            { AttackType.BloodSlash, new Ability("Blood Slash", 10f, 0.5f, AbilityEffect.Damage) },
            { AttackType.DarkWave, new Ability("Dark Wave", 15f, 1f, AbilityEffect.AreaDamage) },
            { AttackType.BatSwarm, new Ability("Bat Swarm", 20f, 2f, AbilityEffect.DamageOverTime) },
            { AttackType.ShadowStep, new Ability("Shadow Step", 0f, 0.3f, AbilityEffect.Teleport) },
            { AttackType.VampireBite, new Ability("Vampire Bite", 25f, 1.5f, AbilityEffect.LifeSteal) },
            { AttackType.SoulReap, new Ability("Soul Reap", 30f, 2f, AbilityEffect.InstantKillLowHP) },
            { AttackType.NightMist, new Ability("Night Mist", 15f, 1f, AbilityEffect.Slow) },
            { AttackType.DeathSpike, new Ability("Death Spike", 20f, 0.8f, AbilityEffect.Pierce) },
            { AttackType.BloodNova, new Ability("Blood Nova", 40f, 3f, AbilityEffect.BigAreaDamage) },
            { AttackType.GrimPulse, new Ability("Grim Pulse", 25f, 1.2f, AbilityEffect.Stun) }
        };
    }

    public void ExecuteAttack(AttackType type)
    {
        if (!abilities.ContainsKey(type)) return;

        Ability ability = abilities[type];

        if (comboTimer > 0)
        {
            activeCombo.Add(ability);
        }
        else
        {
            ExecuteSingleAttack(ability);
            comboTimer = COMBO_WINDOW;
            activeCombo.Add(ability);
        }
    }


    private void ExecuteCombo()
    {
        if (activeCombo.Count > 1)
        {
            //GD.Print($"Executing combo with {activeCombo.Count} abilities!");
            float totalDamage = 0f;
            foreach (var ability in activeCombo)
            {
                totalDamage += ability.Value;
            }
            // Add combo effects and enhanced damage logic here
        }
    }


    private void ExecuteSingleAttack(Ability ability)
    {
        switch (ability.Effect)
        {
            case AbilityEffect.Damage:
                BloodSlashAttack(ability);
                break;
            case AbilityEffect.AreaDamage:
                DarkWaveAttack(ability);
                break;
            case AbilityEffect.DamageOverTime:
                BatSwarmAttack(ability);
                break;
                // Add cases for other abilities
        }
        ability.Use(Time.GetTicksMsec() / 1000f);
    }

    private void BloodSlashAttack(Ability ability)
    {
        Node2D effect = bloodSlashEffect.Instantiate<Node2D>();
        effect.Position = player.Position + (player.GetNode<Sprite2D>("Sprite").FlipH ? new Vector2(-50, 0) : new Vector2(50, 0));
        GetParent().AddChild(effect);

        Area2D hitbox = effect.GetNode<Area2D>("Hitbox");
        foreach (var body in hitbox.GetOverlappingBodies())
        {
            if (body is Enemy enemy)
            {
                enemy.TakeDamage((int)ability.Value);
                if (enemy.Health <= 0) gameManager.EnemyDefeated();
            }
        }
    }

    private void DarkWaveAttack(Ability ability)
    {
        Node2D effect = darkWaveEffect.Instantiate<Node2D>();
        effect.Position = player.Position;
        GetParent().AddChild(effect);

        Area2D hitbox = effect.GetNode<Area2D>("Hitbox");
        foreach (var body in hitbox.GetOverlappingBodies())
        {
            if (body is Enemy enemy)
            {
                enemy.TakeDamage((int)ability.Value);
                if (enemy.Health <= 0) gameManager.EnemyDefeated();
            }
        }
    }

    private void BatSwarmAttack(Ability ability)
    {
        // Implement DOT effect with particles
        // Add similar implementations for other attacks
    }
}