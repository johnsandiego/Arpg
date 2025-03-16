using Godot;
using Godot.Collections;

public partial class PlayerStats : Resource
{
    [Export] public int Level { get; set; } = 1;
    [Export] public int Experience { get; set; } = 0;
    [Export] public int MaxHP { get; set; } = 100;
    [Export] public int CurrentHP { get; set; } = 100;
    [Export] public int MaxMana { get; set; } = 50;
    [Export] public int CurrentMana { get; set; } = 50;
    [Export] public Dictionary<string, int> Kills { get; set; } = new Dictionary<string, int>();
    [Export] public Array<string> LearnedSpells { get; set; } = new Array<string>();

    public int ExperienceToNextLevel => Level * 100; // Simple leveling curve

    public void GainExperience(int amount)
    {
        Experience += amount;
        while (Experience >= ExperienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        Level++;
        MaxHP += 10;
        CurrentHP = MaxHP;
        MaxMana += 5;
        CurrentMana = MaxMana;
        GD.Print($"Leveled up to {Level}!");
    }

    public void KillEnemy(string enemyType)
    {
        Kills[enemyType] = Kills.ContainsKey(enemyType) ? Kills[enemyType] + 1 : 1;
        float chance = Kills[enemyType] * 0.1f; // 10% chance per kill
        if (GD.Randf() < chance)
        {
            LearnSpell(enemyType);
        }
    }

    private void LearnSpell(string enemyType)
    {
        string spell = $"{enemyType}Spell"; // e.g., "FireSpell"
        if (!LearnedSpells.Contains(spell))
        {
            LearnedSpells.Add(spell);
            GD.Print($"Learned {spell}!");
        }
    }
}
