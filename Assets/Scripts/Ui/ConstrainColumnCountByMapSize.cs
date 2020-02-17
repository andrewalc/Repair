using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstrainColumnCountByMapSize : MonoBehaviour
{
    private GridLayoutGroup gridLayout;
    [SerializeField] private float gridSizeMultiplier = 1.0f;
    [SerializeField] private int gridCountOffset = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Game.Instance.LevelGenerated += OnLevelGenerated;
        if (Game.Instance.FinishedGeneratingLevel)
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
        if (null == gridLayout)
        {
            gridLayout = GetComponent<GridLayoutGroup>();
        }

        float gridSize = (sim.currentState.Width * gridSizeMultiplier);
        gridLayout.constraintCount = (int)gridSize + gridCountOffset;
    }
}
