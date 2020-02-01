using System;
using System.Collections.Generic;
using DarkConfig;
using UnityEngine;

public class Game : MonoBehaviour {
    public static Game Instance { get; private set; }

    public bool finishedLoadingConfigs { get; private set; }

    Dictionary<string, CarGrid> testGrids = new Dictionary<string, CarGrid>();

    void LoadConfigs() {
        UnityPlatform.Setup();
        Config.FileManager.AddSource(new ResourcesSource(hotload: true));
        Config.Preload();
        Config.OnPreload += () => {
            finishedLoadingConfigs = true;
        };
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Can only have one game instance!");
        }
        
        LoadConfigs();
    }
}
