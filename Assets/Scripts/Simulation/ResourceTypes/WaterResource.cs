public class WaterResource : ResourceEntry
{
    public WaterResource(float max) : base(max)
    {
    }

    public override ResourceType TypeID
    {
        get { return ResourceType.Water; }
    }
}