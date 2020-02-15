public class PlantMatterResource : ResourceEntry
{
    public PlantMatterResource(float max) : base(max)
    {
    }

    public PlantMatterResource(float max, float startingValue) : base(max, startingValue)
    {
    }

    public override ResourceType TypeID
    {
        get { return ResourceType.PlantMatter; }
    }
}