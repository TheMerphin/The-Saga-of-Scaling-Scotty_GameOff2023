using System;

public abstract class Range<T> where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
{
    public T Min { get; set; }
    public T Max { get; set; }

    public Range(T min, T max)
    {
        Min = min; Max = max;
    }

    public abstract bool IsInRange(T value);
}
