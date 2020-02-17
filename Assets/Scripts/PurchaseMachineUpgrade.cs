using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseMachineUpgrade : MonoBehaviour
{
    void OnMouseOver()
    {
        if (!UiDisable.Instance.disabled && Input.GetMouseButtonDown(0))
        {
            MachineObject machineState = GetComponent<MachineObject>();
            MachineCarObject square = (MachineCarObject) machineState.square.ContainedObject;

            float plantMatter = Game.Instance.Simulation.currentState.GetResourceValue(ResourceType.PlantMatter);
            float upgradeCost = Game.Instance.SimulationSettings.avgReclaimCost;

            if (square.level > 0)
            {
                if (plantMatter >= upgradeCost)
                {
                    Game.Instance.Simulation.currentState.UpgradeMachineAt(machineState.square.X,
                        machineState.square.Y);
                    Game.Instance.Simulation.currentState.ChangeResource(ResourceType.PlantMatter, -upgradeCost);
                }
                else
                {
                    SoundManager.Instance.PlaySound(SoundNames.notEnoughPlantMatter);
                }
            }
        }
    }
}