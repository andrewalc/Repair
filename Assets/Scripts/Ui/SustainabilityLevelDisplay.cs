using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SustainabilityLevelDisplay : TextIntDisplay
{

    protected override int GetAmount()
    {
        return (int)Game.Instance.Simulation.currentState.Sustainability;
    }
}