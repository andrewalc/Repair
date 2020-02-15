public class PlantMatterResource : ResourceEntry
{
    public PlantMatterResource(float max) : base(max)
    {
    }

    public override ResourceType TypeID
    {
        get { return ResourceType.PlantMatter; }
    }
}