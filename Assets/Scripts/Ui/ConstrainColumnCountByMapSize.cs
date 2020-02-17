using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstrainColumnCountByMapSize : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private float gridSizeMultiplier = 1.0f;
    [SerializeField] private int gridCountOffset = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Game.Instance.LevelGenerated += OnLevelGenerated;
        if (Game.Instance.finishedGeneratingLevel &&
            null != Game.Instance.Simulation)
        {
            OnLevelGenerated(Game.Instance.Simulation);
        }
    }

    private void OnDisable()
    {
        Game.Instance.LevelGenerated -= OnLevelGenerated;
    }

    private void OnLevelGenerated(Simulation sim)
    {
        gridLayout.constraintCount = (int)(sim.currentState.Width * gridSizeMultiplier) + gridCountOffset;
    }
}
