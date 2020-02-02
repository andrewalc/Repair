using UnityEngine;
using UnityEngine.UI;

public class PipeButton : MonoBehaviour {
    [SerializeField] Sprite pipeSprite;
    [SerializeField] Sprite emptySprite;

    [SerializeField] Image ButtonImage;

    public void Init(bool empty) {
        ButtonImage.sprite = empty ? emptySprite : pipeSprite;
    }
}

