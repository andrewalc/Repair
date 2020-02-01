using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    void Start()
    {
        WaterResource.value = 10;
        WaterResource.rate = -1;
        WaterResource.max = 10;
    }
}
