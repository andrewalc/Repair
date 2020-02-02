using UnityEngine;
using UnityEngine.UI;

public class PlantMatterDisplay : TextIntDisplay
{
    protected override int GetAmount()
    {
        return (int)(Game.Instance.Simulation.currentState.plantMatter);
    }
}