public static class WaterResource
{
    public static float value;
    public static float max;

    public static void updateWater(float amount)
    {
        value += amount;
        
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
