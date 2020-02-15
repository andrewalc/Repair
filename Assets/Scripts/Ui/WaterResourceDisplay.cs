using UnityEngine;
using UnityEngine.UI;

public class WaterResourceDisplay : ResourceDisplay
{
    protected override ResourceType TypeID
    {
        get { return ResourceType.Water; }
    }

    protected override float UpdateLevelToDisplay(Simulation sim)
    {
        return sim.currentState.GetResourceValue(ResourceType.Water);
    }
}