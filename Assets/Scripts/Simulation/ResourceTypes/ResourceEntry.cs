public abstract class ResourceEntry
{
    public abstract ResourceType TypeID { get; }

    private float value;
    private float max;

    public ResourceEntry(float max)
    {
        this.max = max;
    }
    
    public ResourceEntry(float max, float startingValue)
    {
        this.max = max;
        this.value = startingValue;
    }

    public void Change(float change)
    {
        Set(value + change);
    }

    public void Set(float amount)
    {
        value = amount;
    
        if (value < 0)
        {
            value = 0;
        }

        if (value > max)
        {
            value = max;
        }
    }
    
    public float GetValue()
    {
        return value;
    }
}