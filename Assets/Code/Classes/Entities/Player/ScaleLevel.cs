public enum ScaleLevel
{
    Small = -1,
    Normal = 0, // Default state
    Big = 1
}

public static class ScaleLevelUtils
{
    public static PlayerScalingInfo GetScaleStructByScaleLevel(ScaleLevel scaleLevel)
    {
        switch (scaleLevel)
        {
            case ScaleLevel.Small:
                return new PlayerScalingInfo(ScaleLevel.Small, 0.5f, 1.1f, 1.1f, 0.8f, 1.4f);
            case ScaleLevel.Big:
                return new PlayerScalingInfo(ScaleLevel.Big, 1.75f, 0.8f, 0.8f, 1.2f, 0.6f);
            default: // ScaleLevel.Normal
                return new PlayerScalingInfo(ScaleLevel.Normal, 1f, 1f, 1f, 1f, 1f);
        }
    }
}
