using System;
using UnityEngine;
using UnityEngine.UI;

public enum pipeState {
    Empty,
    Pipe,
    Sprinkler
}

public class PipeButton : MonoBehaviour {
    int x, y;
    pipeState state = pipeState.Empty;

    [SerializeField] Sprite pipeSprite;
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite sprinklerSprite;

    [SerializeField] Image ButtonImage;
    
    public void Init(pipeState state, int x, int y) {
        this.state = state;
        this.x = x;
        this.y = y;
        UpdateSprite();
    }

    public void Toggle() {
        if ((x % 2 == 0) != (y % 2 == 0)) {
            state = state == pipeState.Empty ? pipeState.Pipe : pipeState.Empty;
        } else {
            state = state == pipeState.Empty ? pipeState.Sprinkler : pipeState.Empty;
        }

        UpdateSprite();
    }

    void UpdateSprite() {
        switch (state) {
            case pipeState.Empty: ButtonImage.sprite = emptySprite; break;
            case pipeState.Pipe: ButtonImage.sprite = pipeSprite; break;
            case pipeState.Sprinkler: ButtonImage.sprite = sprinklerSprite; break;
            default: throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
}

