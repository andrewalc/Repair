using UnityEngine;
using UnityEngine.UI;

public class AirResourceDisplay : ResourceDisplay
{
    protected override float UpdateLevelToDisplay()
    {
        return Game.Instance.Simulation.currentState.airQuality;
    }
}