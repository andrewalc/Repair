using System.Collections;
using DarkConfig;
using UnityEngine;

public class Game : MonoBehaviour {
    public static Game Instance { get; private set; }

    public bool finishedLoadingConfigs { get; private set; }

    public SimulationSettingsConfig SimulationSettings = new SimulationSettingsConfig();
    Simulation simulation;

    void LoadConfigs() {
        UnityPlatform.Setup();
        Config.FileManager.AddSource(new ResourcesSource(hotload: true));
        Config.Preload();
        Config.OnPreload += () => {
            Config.Apply("simulationSettings", ref SimulationSettings);
            finishedLoadingConfigs = true;
        };
    }

    IEnumerator Start() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Can only have one game instance!");
        }
        
        LoadConfigs();

        yield return new WaitUntil(() => finishedLoadingConfigs);
        
        simulation = new Simulation(SimulationSettings.testGrids[0], SimulationSettings);
        Debug.Log(simulation.currentState);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            simulation.Step();
            Debug.Log(simulation.currentState);
        }
    }
}
