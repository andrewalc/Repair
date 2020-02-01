public static class WaterResource
{
    public static float value;
    public static float rate;
    public static float max;

    public static void Increment()
    {
        value += rate;
        
        if (value < 0)
        {
            value = 0;
        }

        if (value > max)
        {
            value = max;
        }
    }
}
