using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
    public TextMeshProUGUI t1;
    public TextMeshProUGUI t2;
    public TextMeshProUGUI t3;
    public TextMeshProUGUI t4;
    public static HoverManager Instance { get; private set; }
    
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        Deactivate();
    }

    public void Display(GridSquare square)
    {
        if (!Game.Instance.finishedLoadingConfigs)
        {
            return;
        }
        
        if (square.ContainedObject.Type == CarObjectType.Plant)
        {
            PlantCarObject plant = (PlantCarObject) square.ContainedObject;

            t1.text = "";
            t2.text = "Health: " + plant.health;
            t3.text = "";
            t4.text = "";
        }
        if (square.ContainedObject.Type == CarObjectType.Machine)
        {
            MachineCarObject machine = (MachineCarObject) square.ContainedObject;

            float pollution;
            float watergen;
            int level = machine.level;
            float cost = Game.Instance.SimulationSettings.avgReclaimCost;
            
            
            if (machine.MachineType == MachineCarObject.MachineTypes.Aero)
            {
                pollution = Game.Instance.SimulationSettings.aeroPollutionRate;
                watergen = Game.Instance.SimulationSettings.aeroWaterGenRate;
            }
            else
            {
                pollution = Game.Instance.SimulationSettings.hydroPollutionRate;
                watergen = Game.Instance.SimulationSettings.hydroWaterGenRate;
            }

            t1.text = "AirQuality/tick: " + pollution;
            t2.text = "Water/tick: " + watergen;
            t3.text = "Level: " + level;
            t4.text = "Cost: " + cost;
        }
        
        transform.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        transform.gameObject.SetActive(false);
    }
}
