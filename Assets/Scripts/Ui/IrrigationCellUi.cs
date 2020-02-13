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
                    break;
                }
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

        gameState.Sprinklers[x, y] = true;
        this.sprinkler = gameState.Sprinklers[x, y];
        gameState.plantMatter -= Game.Instance.SimulationSettings.sprinklerPrice;

        UpdateState();
    }
}