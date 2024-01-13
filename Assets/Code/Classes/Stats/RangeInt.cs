public class RangeInt : Range<int>
{
    public RangeInt(int min, int max) : base(min, max) { }

    public override bool IsInRange(int value)
    {
        return value >= Min && value <= Max;
    }


    public static RangeInt of(int min, int max)
    {
        return new RangeInt(min, max);
    }
}
