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
        if (square.ContainedObject.Type == CarObjectType.Plant)
        {
            PlantCarObject plant = (PlantCarObject) square.ContainedObject;

            t1.text = "";
            t2.text = "Health: " + plant.health;
            t3.text = "";
        }
        if (square.ContainedObject.Type == CarObjectType.Machine)
        {
            MachineCarObject machine = (MachineCarObject) square.ContainedObject;

            float pollution;
            float watergen;
            int level = machine.level;
            
            if (machine.MachineType == MachineCarObject.MachineTypes.Aero)
            {
                pollution = 2;
                watergen = 3;
            }
            else
            {
                pollution = 1;
                watergen = 5;
            }

            t1.text = "AirQuality/sec: " + pollution;
            t2.text = "Water/sec: " + watergen;
            t3.text = "Level: " + level;
        }
        
        transform.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        transform.gameObject.SetActive(false);
    }
}
