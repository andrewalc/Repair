using UnityEngine;
using UnityEngine.UI;

public class WaterResourceDisplay : ResourceDisplay
{
    protected override float UpdateLevelToDisplay()
    {
        return Game.Instance.Simulation.currentState.waterLevel;
    }
}