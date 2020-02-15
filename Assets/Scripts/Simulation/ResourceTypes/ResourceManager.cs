using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    private Dictionary<ResourceType, ResourceEntry> resources = new Dictionary<ResourceType, ResourceEntry>();

    public event Simulation.ResourceChangedEvent ResourceChanged;
    
    void Start()
    {
    }

    public ResourceEntry AddToResource(ResourceType type, float value)
    {
        if (!Game.Instance.finishedLoadingConfigs)
        {
            return null;
        }
        
        if (!resources.TryGetValue(type, out var resource))
        {
            resource = CreateResourceType(type);
            resources[type] = resource;
        }

        float oldValue = resource.GetValue();
        resource.Change(value);
        
        ResourceChanged?.Invoke(oldValue, resource);

        return resource;
    }

    public ResourceEntry SetResource(ResourceType type, float value)
    {
        if (!Game.Instance.finishedLoadingConfigs)
        {
            return null;
        }
        
        if (!resources.TryGetValue(type, out var resource))
        {
            resource = CreateResourceType(type);
            resources[type] = resource;
        }

        float oldValue = resource.GetValue();
        resource.Set(value);
        
        ResourceChanged?.Invoke(oldValue, resource);

        return resource;
    }
    
    public ResourceEntry Get(ResourceType type)
    {
        if (!Game.Instance.finishedLoadingConfigs)
        {
            return null;
        }
        
        if (!resources.TryGetValue(type, out var resource))
        {
            resource = CreateResourceType(type);
        }

        return resource;
    }

    public float GetValue(ResourceType type)
    {
        if (!Game.Instance.finishedLoadingConfigs)
        {
            return 0.0f;
        }

        if (!resources.TryGetValue(type, out var resource))
        {
            return 0.0f;
        }

        return resource.GetValue();
    }

    private ResourceEntry CreateResourceType(ResourceType type)
    {
        float maxOfType = GetMaxForType(type);

        switch (type)
        {
            case ResourceType.Water:
                return new WaterResource(maxOfType);
            case ResourceType.AirQuality:
                return new AirQualityResource(maxOfType);
            case ResourceType.PlantMatter:
                return new PlantMatterResource(maxOfType);
            default:
                throw new System.NotImplementedException();
        }
    }

    private float GetMaxForType(ResourceType type)
    {
        float value = 0;
        if (!Game.Instance.SimulationSettings.maxResourceValues.TryGetValue(type, out value))
        {
            Debug.LogError("No max configured for resource: " + type);
            return 0.0f;
        }

        return value;
    }

    public void CopyFrom(ResourceManager other)
    {
        foreach (ResourceEntry resource in other.resources.Values)
        {
            SetResource(resource.TypeID, resource.GetValue());
        }
    }
}