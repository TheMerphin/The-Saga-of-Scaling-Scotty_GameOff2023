public class EntityStatFloat : EntityStat<float>
{
    public EntityStatFloat(string displayName, RangeFloat valueRange, float initialValue, params StatModifier[] modifiers) : base(displayName, valueRange, initialValue, modifiers) {}

    public override float CalculateValue()
    {
        var value = BaseValue;

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

    public override Range<float> CalculateValueRange()
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
        if (ValueRange.IsInRange(BaseValue + additive))
        {
            // Decrease TemporaryAdditive first
            if (additive < 0 && TemporaryAdditive > 0)
            {
                TemporaryAdditive += additive;

                if (TemporaryAdditive < 0)
                {
                    additive = TemporaryAdditive;
                    TemporaryAdditive = 0;
                }
            }

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
