using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SustainabilityLevelDisplay : TextIntDisplay
{

    protected override int GetAmount(Simulation sim)
    {
        return (int)sim.currentState.Sustainability;
    }
}