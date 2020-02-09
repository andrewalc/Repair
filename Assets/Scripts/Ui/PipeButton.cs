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

        if (x % 2 == 1 && y % 2 == 1) {
            Destroy(GetComponent<Button>());
            Destroy(GetComponent<Image>());
        } else {
            UpdateSprite();
        }
    }

    public void Toggle() {
        var gameState = Game.Instance.Simulation.currentState;
        
        if ((x % 2 == 0) != (y % 2 == 0)) {
            
            // can we afford it?
            if (gameState.plantMatter < Game.Instance.SimulationSettings.pipePrice) {
                Debug.Log("Can't afford the pipe");
                return;
            }
            
            gameState.plantMatter -= Game.Instance.SimulationSettings.pipePrice;

            if (y % 2 == 1) {
                gameState.AddPipeBetween(x / 2, (y - 1) / 2, x / 2, (y + 1) / 2);
            } else {
                gameState.AddPipeBetween((x - 1) / 2, y / 2, (x + 1) / 2, y / 2);
            }
            
            state = state == pipeState.Empty ? pipeState.Pipe : pipeState.Empty;
        } else {
            // TODO can we place a sprinkler here?

            if (gameState.Squares[x / 2, y / 2].ContainedObject.Type == CarObjectType.Plant) {
                // can we afford it?
                if (gameState.plantMatter < Game.Instance.SimulationSettings.sprinklerPrice) {
                    Debug.Log("Can't afford the sprinkler");
                    return;
                }
                
                gameState.Sprinklers[x / 2, y / 2] = !gameState.Sprinklers[x / 2, y / 2];
                gameState.plantMatter -= Game.Instance.SimulationSettings.sprinklerPrice;
                state = gameState.Sprinklers[x / 2, y / 2] ? pipeState.Sprinkler : pipeState.Empty;
            }
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

        if ((x % 2 == 0) == (y % 2 == 0) && x % 2 == 0) {
            var gameState = Game.Instance.Simulation.currentState;
            if (gameState.Squares[x / 2, y / 2].ContainedObject.Type == CarObjectType.Plant) {
                ButtonImage.color =
                    state == pipeState.Sprinkler ?
                    new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.1f);
            } else {
                ButtonImage.color = new Color(1, 1, 1, 0f);
            }
        } else if (state == pipeState.Empty) { 
            ButtonImage.color = new Color(1, 1, 1, 0.1f);
        } else {
            ButtonImage.color = new Color(1, 1, 1, 1);
        }
    }
}

