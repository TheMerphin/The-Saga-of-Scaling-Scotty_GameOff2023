using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityStat
{
    public string DisplayName { get; set; }

    public List<StatModifier> Modifiers;

    public void AddModifier(StatModifier modifier)
    {
        if (Modifiers.Find(x => x.Name.Equals(modifier.Name)) != null) Debug.LogWarning("Adding another value modifier with the same name. Removing them in the future results in both being removed.");

        Modifiers.Add(modifier);
        Modifiers.Sort((x, y) => x.Type.CompareTo(y.Type));
    }

    public bool RemoveModifier(string identifier)
    {
        return Modifiers.RemoveAll(x => x.Equals(identifier)) > 0;
    }
}

/**
 * A value storing class, which enables to represent single status values.
 * Offers methods with which the value can temporally be modified, e.g. 
 * when the player scales or when he's carrying an item which grants more life.
 * 
 * Modifiers always affect the current base value and the respective maximal range value.
 * Removing modifiers will not lead to numbers below the minimal value.
 */
public abstract class EntityStat<T> : EntityStat where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
{
    public Range<T> ValueRange { get { return CalculateValueRange(); } set { ValueRange = value; } }

    public T Value { get { return CalculateValue(); } set { Value = value; } }

    // Can exceed the total value beyond max value range
    public T TemporaryAdditive { get; set; }

    public EntityStat(string displayName, Range<T> valueRange, T initialValue, params StatModifier[] modifiers)
    {
        DisplayName = displayName;
        ValueRange = valueRange;
        Value = initialValue;

        Modifiers = new List<StatModifier>();
        Modifiers.AddRange(modifiers);
        Modifiers.Sort((x, y) => x.Type.CompareTo(y.Type));
    }

    protected abstract T CalculateValue();

    protected abstract Range<T> CalculateValueRange();

    public abstract void Update(T additive);
}
