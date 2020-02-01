using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    void Start()
    {
        WaterResource.value = 10;
        WaterResource.max = 10;
        AirQualityResource.value = 50;
        AirQualityResource.max = 100;
    }
}
