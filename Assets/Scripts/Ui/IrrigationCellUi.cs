using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IrrigationCellUi : MonoBehaviour
{
    public GridSquare square;

    private Image img;

    public StringToSprite[] spriteMap;
    [System.Serializable] 
    
    public class StringToSprite
    {
        /// Name of the variable
        public string name;
        /// Value of the variable
        public Sprite sprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Render();
    }

    void Render()
    {
        string name = "";
        switch (square.ContainedObject.Type)
        {
            case CarObjectType.Machine: {}
                name = "machine";
                break;
            case CarObjectType.Obstacle:
                name = "obstacle";
                break;
            case CarObjectType.Plant:
                name = "plant";
                break;
            case CarObjectType.Spigot:
                name = "spigot";
                break;
            case CarObjectType.Empty:
                name = "pipe";
                break;
            default:
                name = "sprinkler";
                break;
        }
        foreach (StringToSprite sts in spriteMap)
        {
            if (sts.name != name) continue;
            img.sprite = sts.sprite;
            break;
        }
    }
}
