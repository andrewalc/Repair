using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
	[Header("Machine")]
	[SerializeField] private GameObject machinePanel;
	[SerializeField] private TextMeshProUGUI t1;
	[SerializeField] private TextMeshProUGUI t2;
	[SerializeField] private TextMeshProUGUI t3;
	[SerializeField] private TextMeshProUGUI t4;
	[Header("Plant")]
	[SerializeField] private GameObject plantPanel;
	[SerializeField] private Image plantHealthDisplay;
	[SerializeField] private Sprite badSprite;
	[SerializeField] private Sprite mediumSprite;
	[SerializeField] private Sprite goodSprite;
	[SerializeField] private Gradient healthGradient;

	[SerializeField] private Color notEnoughPlantMatterColor;
	[SerializeField] private Color noUpgradeAvailableColor;
	[SerializeField] private Color canUpgradeColor;

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

    public void Display(GridSquare square)
    {
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
			plantPanel.SetActive(true);

			if (plant.health < 50)
			{
				plantHealthDisplay.sprite = badSprite;
			}
			else if (plant.health < 100)
			{
				plantHealthDisplay.sprite = mediumSprite;
			}
			else
			{
				plantHealthDisplay.sprite = goodSprite;
			}
			plantHealthDisplay.color = healthGradient.Evaluate(plant.health / 150);
		}
        
        if (square.ContainedObject.Type == CarObjectType.Machine)
        {
            MachineCarObject machine = (MachineCarObject) square.ContainedObject;
			machinePanel.SetActive(true);

            float pollution;
            float watergen;
            int level = machine.level;
            float cost = settings.avgReclaimCost;
            
            if (machine.MachineType == MachineCarObject.MachineTypes.Aero)
            {
                pollution = (settings.maxMachineLevel - machine.level) * settings.aeroPollutionRate;
                watergen = settings.aeroWaterGenRate * (machine.level - 1);
            }
            else
            {
                pollution = (settings.maxMachineLevel - machine.level) * settings.hydroPollutionRate;
                watergen = settings.hydroWaterGenRate * (machine.level - 1);
            }

            t1.text = "Pollution Rate: " + pollution.ToString("F2");
            t2.text = "Water Rate: " + watergen.ToString("F2");
            t3.text = "Level: " + level + "/" + settings.maxMachineLevel;
            t4.text = "Cost: " + cost;

            if (level >= settings.maxMachineLevel)
            {
	            t4.color = noUpgradeAvailableColor;
            }
            else if (Game.Instance.CurrGrid.GetResourceValue(ResourceType.PlantMatter) < cost)
            {
	            t4.color = notEnoughPlantMatterColor;
            }
            else
            {
	            t4.color = canUpgradeColor;
            }
        }
        
        transform.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        transform.gameObject.SetActive(false);
		machinePanel.SetActive(false);
		plantPanel.SetActive(false);
    }
}
