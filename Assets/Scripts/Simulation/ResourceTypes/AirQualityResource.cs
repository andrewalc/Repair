public static class AirQualityResource
{
    public static float value;
    public static float max;

    public static void changeAirQuality(float change)
    {
        value += change;
        
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