// Ability.cs
public enum AbilityEffect
{
    Damage, AreaDamage, DamageOverTime, Teleport, LifeSteal,
    InstantKillLowHP, Slow, Pierce, BigAreaDamage, Stun
}

public class Ability
{
    public string Name { get; private set; }
    public float Value { get; private set; }
    public float Cooldown { get; private set; }
    public AbilityEffect Effect { get; private set; }
    private float lastUsed = 0f;

    public Ability(string name, float value, float cooldown, AbilityEffect effect)
    {
        Name = name;
        Value = value;
        Cooldown = cooldown;
        Effect = effect;
    }

    public bool IsReady(float currentTime)
    {
        return currentTime - lastUsed >= Cooldown;
    }

    public void Use(float currentTime)
    {
        lastUsed = currentTime;
    }
}