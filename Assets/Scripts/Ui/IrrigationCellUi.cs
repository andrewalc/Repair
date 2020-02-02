using System;
using UnityEngine;
using UnityEngine.UI;

public class IrrigationCellUi : MonoBehaviour {
    [Serializable]
    public class StringToSprite {
        /// Name of the variable
        public string name;

        /// Value of the variable
        public Sprite sprite;
    }
    
    [SerializeField] StringToSprite[] spriteMap;

    Image img;

    void Awake() {
        img = GetComponent<Image>();
    }

    public void Init(CarObjectType type) {
        string spriteName;
        switch (type) {
            case CarObjectType.Machine:
                spriteName = "machine";
                break;
            case CarObjectType.Obstacle:
                spriteName = "obstacle";
                break;
            case CarObjectType.Plant:
                spriteName = "plant";
                break;
            case CarObjectType.Spigot:
                spriteName = "spigot";
                break;
            case CarObjectType.Empty:
                spriteName = null;
                break;
            default:
                spriteName = null;
                break;
        }

        // TODO Sprinklers

        if (spriteName == null) {
            img.sprite = null;
        } else {
            foreach (var sts in spriteMap) {
                if (sts.name == spriteName) {
                    img.sprite = sts.sprite;
                    break;
                }
            }
        }
    }
}
