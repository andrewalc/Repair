using System;
using UnityEngine;
using UnityEngine.UI;

public enum PipeState
{
    Empty,
    Pipe,
    Sprinkler
}

public class PipeButton : MonoBehaviour
{
    int x, y;
    PipeState state = PipeState.Empty;

    [SerializeField] Sprite pipeSprite;
    [SerializeField] Sprite emptySprite;

    [SerializeField] Image ButtonImage;

    public void Init(PipeState state, int x, int y)
    {
        this.state = state;
        this.x = x;
        this.y = y;

        if (x % 2 == 1 && y % 2 == 1 ||
            x % 2 == 0 && y % 2 == 0)
        {
            Destroy(GetComponent<Button>());
            Destroy(GetComponent<Image>());
        }
        else
        {
            if (y % 2 == 1)
            {
                // We are a vertical pipe. Rotate.
                this.transform.Rotate(Vector3.forward, 90.0f);
            }
            UpdateSprite();
        }
    }

    public void Toggle()
    {
        var gameState = Game.Instance.Simulation.currentState;

        if ((x % 2 == 0) == (y % 2 == 0))
        {
            // This is not a pipe, but an empty space or possible sprinkler.
            return;
        }

        // can we afford it?
        if (gameState.plantMatter < Game.Instance.SimulationSettings.pipePrice)
        {
            Debug.Log("Can't afford the pipe");
            return;
        }

        if (state == PipeState.Pipe)
        {
            // Can't add a pipe in a place where we already have one.
            return;
        }

        gameState.plantMatter -= Game.Instance.SimulationSettings.pipePrice;

        if (y % 2 == 1)
        {
            gameState.AddPipeBetween(x / 2, (y - 1) / 2, x / 2, (y + 1) / 2);
        }
        else
        {
            gameState.AddPipeBetween((x - 1) / 2, y / 2, (x + 1) / 2, y / 2);
        }

        state = state == PipeState.Empty ? PipeState.Pipe : PipeState.Empty;
        UpdateSprite();
    }

    void UpdateSprite()
    {
        switch (state)
        {
            case PipeState.Pipe:
                ButtonImage.sprite = pipeSprite;
                break;
            default:
                ButtonImage.sprite = emptySprite;
                break;
        }

        if ((x % 2 == 0) == (y % 2 == 0) && x % 2 == 0)
        {
            // Do nothing - this is not a pipe, but a sprinkler.
        }
        else if (state == PipeState.Empty)
        {
            ButtonImage.color = new Color(1, 1, 1, 0.7f);
        }
        else
        {
            ButtonImage.color = new Color(1, 1, 1, 1);
        }
    }
}