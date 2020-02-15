using System;
using System.Net;
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

    [SerializeField] private Color minWaterColor;

    [SerializeField] private Color maxWaterColor;

    [SerializeField] private Color noWaterColor;
    
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
            state = PipeState.Empty;
        }
        else
        {
            if (y % 2 == 1)
            {
                // We are a vertical pipe. Rotate.
                this.transform.Rotate(Vector3.forward, 90.0f);
            }

            UpdateSprite();

            Game.Instance.OnSimTickFinished += OnSimTick;
        }
    }

    public void OnDestroy()
    {
        Game.Instance.OnSimTickFinished -= OnSimTick;
    }

    private void OnSimTick(Simulation simulation)
    {
        UpdateColor(simulation);    
    }

    public void Toggle()
    {
        var gameState = Game.Instance.Simulation.currentState;

        if ((x % 2 == 0) == (y % 2 == 0))
        {
            // This is not a pipe, but an empty space or possible sprinkler.
            return;
        }

        bool success;
        if (y % 2 == 1)
        {
            success = gameState.TogglePipeBetween(x / 2, (y - 1) / 2, x / 2, (y + 1) / 2);
        }
        else
        {
            success = gameState.TogglePipeBetween((x - 1) / 2, y / 2, (x + 1) / 2, y / 2);
        }

        if (!success)
        {
            return;
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
        
        UpdateColor(Game.Instance.Simulation);
    }

    private void UpdateColor(Simulation simulation)
    {
        if ((x % 2 == 0) == (y % 2 == 0) && x % 2 == 0)
        {
            // Do nothing - this is not a pipe, but a sprinkler.
            return;
        }
        
        if (state == PipeState.Empty)
        {
            ButtonImage.color = noWaterColor;
            return;
        }
        
        float inverseWaterDist = GetInverseWaterDist(simulation.currentState);
        if (inverseWaterDist <= float.Epsilon)
        {
            ButtonImage.color = noWaterColor;
        }
        else
        {
            ButtonImage.color = Color.Lerp(minWaterColor, maxWaterColor, inverseWaterDist);
        }
    }

    private float GetInverseWaterDist(CarGrid state)
    {
        float minDist1 = float.PositiveInfinity;

        float minDist2 = float.PositiveInfinity;
        if (x % 2 == 1)
        {
            minDist1 = state.GetWaterTravelDist(x / 2, y / 2);
            minDist2 = state.GetWaterTravelDist(x / 2 + 1, y / 2);
        }

        else
        {
            minDist1 = state.GetWaterTravelDist(x / 2, y / 2);
            minDist2 = state.GetWaterTravelDist(x / 2, y / 2 + 1);
        }

        if (float.IsInfinity(minDist1) || float.IsInfinity(minDist2))
        {
            return 0;
        }

        float avgDist = (minDist1 + minDist2) / 2;
        return 1.0f - (avgDist / (state.Width * state.Height));
    }
}