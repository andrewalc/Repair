using UnityEngine;
using UnityEngine.UI;

public class AirResourceDisplay : ResourceDisplay
{
    protected override ResourceType TypeID
    {
        get { return ResourceType.AirQuality; }
    }

    protected override float UpdateLevelToDisplay(Simulation sim)
    {
        return sim.currentState.GetResourceValue(ResourceType.AirQuality);
    }
}