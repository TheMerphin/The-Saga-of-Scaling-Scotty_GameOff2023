public class EntityStatFloat : EntityStat<float>
{
    public EntityStatFloat(string displayName, RangeFloat valueRange, float initialValue, params StatModifier[] modifiers) : base(displayName, valueRange, initialValue, modifiers) {}

    protected override float CalculateValue()
    {
        var value = Value;

        Modifiers.ForEach(modifierEntry => {
            switch (modifierEntry.Type)
            {
                case StatModifier.ModifierType.Additive:
                    break;
                case StatModifier.ModifierType.Multiplicative:
                    value *= modifierEntry.Value;
                    break;
            }
        });

        return value + TemporaryAdditive;
    }

    protected override Range<float> CalculateValueRange()
    {
        var valueRange = ValueRange;

        Modifiers.ForEach(modifierEntry => {
            switch (modifierEntry.Type)
            {
                case StatModifier.ModifierType.Additive:
                    valueRange.Max += modifierEntry.Value;
                    break;
                case StatModifier.ModifierType.Multiplicative:
                    valueRange.Max *= modifierEntry.Value;
                    break;
            }
        });

        return valueRange;
    }

    public override void Update(float additive)
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
