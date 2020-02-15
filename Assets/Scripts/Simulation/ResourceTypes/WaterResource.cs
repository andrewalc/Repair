public class WaterResource : ResourceEntry
{
    public WaterResource(float max) : base(max)
    {
    }

    public WaterResource(float max, float startingValue) : base(max, startingValue)
    {
    }

    public override ResourceType TypeID
    {
        get { return ResourceType.Water; }
    }
}