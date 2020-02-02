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

    bool _disabled;
    public bool disabled {
        get => _disabled;
        set {
            _disabled = value;
            if (_disabled) {
                Deactivate();
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        Deactivate();
    }

    public void Display(GridSquare desiredSquare)
    {
        // FIXME: This is a terrible hack. (We should already have the right square, why do we need to do this?)
        GridSquare square = Game.Instance.Simulation.currentState.Squares[desiredSquare.X, desiredSquare.Y];

        if (disabled) {
            return;
        }
        
        if (!Game.Instance.finishedLoadingConfigs)
        {
            return;
        }
        
        
        SimulationSettingsConfig settings = Game.Instance.SimulationSettings;
        
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
            float cost = settings.avgReclaimCost;
            
            if (machine.MachineType == MachineCarObject.MachineTypes.Aero)
            {
                pollution = (settings.maxMachineLevel - machine.level) / (float) settings.maxMachineLevel * settings.aeroPollutionRate;
                watergen = settings.aeroWaterGenRate;
            }
            else
            {
                pollution = (settings.maxMachineLevel - machine.level) / (float) settings.maxMachineLevel * settings.hydroPollutionRate;
                watergen = settings.hydroWaterGenRate;
            }

            t1.text = "AirQuality/tick: " + pollution;
            t2.text = "Water/tick: " + watergen;
            t3.text = "Level: " + level + "/" + settings.maxMachineLevel;
            t4.text = "Cost: " + cost;
        }
        
        transform.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        transform.gameObject.SetActive(false);
    }
}
