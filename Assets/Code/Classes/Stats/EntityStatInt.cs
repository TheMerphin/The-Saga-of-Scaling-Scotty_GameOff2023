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

    public override int CalculateValue()
    {
        var value = BaseValue;

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

    public override Range<int> CalculateValueRange()
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
        if (ValueRange.IsInRange(BaseValue + additive))
        {
            BaseValue += additive;
        }
        else if (additive >= 0)
        {
            BaseValue = ValueRange.Max;
        }
        else
        {
            BaseValue = ValueRange.Min;
        }
    }
}
