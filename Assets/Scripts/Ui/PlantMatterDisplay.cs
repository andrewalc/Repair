using UnityEngine;
using UnityEngine.UI;

public class PlantMatterDisplay : TextIntDisplay
{
    protected override void Start()
    {
        base.Start();

        Game.Instance.ResourceChanged += OnResourceChanged;
    }

    private void OnResourceChanged(Simulation sim, float oldValue, ResourceEntry entry)
    {
        if (entry.TypeID != ResourceType.PlantMatter)
        {
            return;
        }
        
        UpdateAmount(sim);
    }
    
    protected override int GetAmount(Simulation sim)
    {
        return (int)(sim.currentState.GetResourceValue(ResourceType.PlantMatter));
    }
}