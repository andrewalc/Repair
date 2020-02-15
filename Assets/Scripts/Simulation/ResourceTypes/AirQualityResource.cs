public class AirQualityResource : ResourceEntry
{
    public AirQualityResource(float max) : base(max)
    {
    }

    public override ResourceType TypeID
    {
        get { return ResourceType.AirQuality; }
    }
}