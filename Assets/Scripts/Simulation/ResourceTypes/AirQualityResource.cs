public class AirQualityResource : ResourceEntry
{
    public AirQualityResource(float max) : base(max)
    {
    }

    public AirQualityResource(float max, float startingValue) : base(max, startingValue)
    {
    }

    public override ResourceType TypeID
    {
        get { return ResourceType.AirQuality; }
    }
}