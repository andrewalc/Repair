using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchaseMachineUpgrade : MonoBehaviour
{
    void OnMouseOver(){
        if(Input.GetMouseButtonDown(0))
        {
            MachineObject machineState = GetComponent<MachineObject>();
            MachineCarObject square = (MachineCarObject) machineState.square.ContainedObject;

            float plantMatter = Game.Instance.Simulation.currentState.plantMatter;
            float upgradeCost = Game.Instance.SimulationSettings.avgReclaimCost;

            if (square.level > 0 && plantMatter > upgradeCost)
            {
                square.level++;
                Game.Instance.Simulation.currentState.plantMatter -= upgradeCost;
            }
        }
    }
}
