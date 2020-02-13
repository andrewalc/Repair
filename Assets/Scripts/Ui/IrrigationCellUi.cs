using System;
using UnityEngine;
using UnityEngine.UI;

public class IrrigationCellUi : MonoBehaviour
{
    [Serializable]
    public class StringToSprite
    {
        /// Name of the variable
        public string name;

        public Color color;
        
        /// Value of the variable
        public Sprite sprite;
    }

    [SerializeField] StringToSprite[] spriteMap;

    private int x;
    private int y;

    Image img;

    private CarObjectType type;

    private bool sprinkler;

    [SerializeField] private Color healthyPlantColor;

    [SerializeField] private Color unhealthyPlantColor;

    [SerializeField] private Color waterFlowingColor;

    [SerializeField] private Color noWaterFlowingColor;
    
    void Awake()
    {
        img = GetComponent<Image>();
    }

    public void Init(int x, int y, CarObjectType type, bool sprinkler)
    {
        this.x = x;
        this.y = y;
        this.type = type;
        this.sprinkler = sprinkler;
        
        UpdateState();
        
        // Register for the SimTick. Prevent double-register.
        Game.Instance.OnSimTickFinished -= OnSimTick;
        Game.Instance.OnSimTickFinished += OnSimTick;
    }
    
    public void OnDisable()
    {
        Game.Instance.OnSimTickFinished -= OnSimTick;
    }

    private void OnSimTick(Simulation sim)
    {
        UpdateColor(sim);
    }
    
    private string DetermineSpriteName()
    {
        string spriteName;
        switch (type)
        {
            case CarObjectType.Machine:
                spriteName = "machine";
                break;
            case CarObjectType.Obstacle:
                spriteName = "obstacle";
                break;
            case CarObjectType.Plant:
                if (sprinkler)
                {
                    spriteName = "plantwithsprinkler";
                }
                else
                {
                    spriteName = "plant";
                }

                break;
            case CarObjectType.Spigot:
                spriteName = "spigot";
                break;
            case CarObjectType.Empty:
                if (sprinkler)
                {
                    spriteName = "sprinkler";
                }
                else
                {
                    spriteName = "border";
                }

                break;
            default:
                spriteName = null;
                break;
        }

        return spriteName;
    }

    private void UpdateState()
    {
        string spriteName = DetermineSpriteName();
        if (spriteName == null)
        {
            img.sprite = null;
            img.color = new Color(0, 0, 0, 0);
        }
        else
        {
            foreach (var sts in spriteMap)
            {
                if (sts.name.Equals(spriteName))
                {
                    img.sprite = sts.sprite;
                    img.color = sts.color;
                    UpdateColor(Game.Instance.Simulation);
                    break;
                }
            }
        }
    }

    private void UpdateColor(Simulation simulation)
    {
        if (type == CarObjectType.Empty)
        {
            if (simulation.currentState.IsWatered(x, y))
            {
                img.color = waterFlowingColor;
            }
            else
            {
                img.color = noWaterFlowingColor;
            }
        }
        else if (type == CarObjectType.Plant)
        {
            if (simulation.currentState.IsWatered(x, y))
            {
                img.color = healthyPlantColor;
            }
            else
            {
                img.color = unhealthyPlantColor;
            }
        }
    }

    public void OnClick()
    {
        var gameState = Game.Instance.Simulation.currentState;

        if (gameState.Sprinklers[x, y])
        {
            return;
        }

        // can we afford it?
        if (gameState.plantMatter < Game.Instance.SimulationSettings.sprinklerPrice)
        {
            Debug.Log("Can't afford the sprinkler");
            return;
        }

        gameState.BuySprinkler(x,y);
        this.sprinkler = gameState.Sprinklers[x, y];

        UpdateState();
    }
}