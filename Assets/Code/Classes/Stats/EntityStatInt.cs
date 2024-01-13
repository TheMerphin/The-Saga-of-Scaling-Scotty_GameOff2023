using System;

public class EntityStatInt : EntityStat<int>
{
    // Enables to define custom rounding behaviour when there are float values through multiplicative modifiers.
    public Func<float, int> RoundFunction { get; set; }

    public EntityStatInt(string displayName, RangeInt valueRange, int initialValue, params StatModifier[] modifiers) : base(displayName, valueRange, initialValue, modifiers)
    {
        RoundFunction = (a) => { return (int)a; };
    }

    public EntityStatInt(string displayName, RangeInt valueRange, int initialValue, Func<float, int> roundFunction, params StatModifier[] modifiers) : base(displayName, valueRange, initialValue, modifiers)
    {
        RoundFunction = roundFunction;
    }

    protected override int CalculateValue()
    {
        var value = Value;

        Modifiers.ForEach(modifierEntry => {
            switch (modifierEntry.Type)
            {
                case StatModifier.ModifierType.Additive:
                    break;
                case StatModifier.ModifierType.Multiplicative:
                    value = RoundFunction.Invoke(modifierEntry.Value * value);
                    break;
            }
        });

        return value + TemporaryAdditive;
    }

    protected override Range<int> CalculateValueRange()
    {
        var valueRange = ValueRange;

        Modifiers.ForEach(modifierEntry => {
            switch (modifierEntry.Type)
            {
                case StatModifier.ModifierType.Additive:
                    valueRange.Max += (int) modifierEntry.Value;
                    break;
                case StatModifier.ModifierType.Multiplicative:
                    valueRange.Max *= RoundFunction.Invoke(modifierEntry.Value * valueRange.Max);
                    break;
            }
        });

        return valueRange;
    }

    public override void Update(int additive)
    {
        if (ValueRange.IsInRange(Value + additive))
        {
            Value += additive;
        }
        else if (additive >= 0)
        {
            Value = ValueRange.Max;
        }
        else
        {
            Value = ValueRange.Min;
        }
    }
}
