using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SustainabilityLevelDisplay : TextIntDisplay
{
	[SerializeField] private Gradient gradient;

    protected override int GetAmount(Simulation sim)
    {
        return (int)sim.currentState.Sustainability;
    }

	protected override void UpdateAmount(Simulation sim)
	{
		base.UpdateAmount(sim);
		textToUpdate.color = gradient.Evaluate(amountToDisplay / 100f);
	}
}