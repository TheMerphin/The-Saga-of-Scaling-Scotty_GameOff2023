public class RangeFloat : Range<float>
{
    public RangeFloat(float min, float max) : base(min, max) { }

    public override bool IsInRange(float value)
    {
        return value >= Min && value <= Max;
    }

    public static RangeFloat of(float min, float max)
    {
        return new RangeFloat(min, max);
    }
}
