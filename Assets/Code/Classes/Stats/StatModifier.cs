
public class StatModifier
{
    public readonly string Name;

    public readonly float Value;

    public readonly ModifierType Type;

    public enum ModifierType
    {
        // Increases the max range ONLY
        Additive = 0,

        // Increases base value AND max range
        Multiplicative = 1
    }

    public StatModifier(string name, float value, ModifierType type)
    {
        Name = name;
        Value = value;
        Type = type;
    }
}
