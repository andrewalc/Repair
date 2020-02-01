﻿using System.Collections;
using DarkConfig;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public bool finishedLoadingConfigs { get; private set; }

    public bool finishedGeneratingLevel { get; private set; }

    private System.Random random;

    public SimulationSettingsConfig SimulationSettings = new SimulationSettingsConfig();
    Simulation simulation;

    public CarGrid currGrid { get; private set; }

    void LoadConfigs()
    {
        UnityPlatform.Setup();
        Config.FileManager.AddSource(new ResourcesSource(hotload: true));
        Config.Preload();
        Config.OnPreload += () =>
        {
            Config.Apply("simulationSettings", ref SimulationSettings);
            finishedLoadingConfigs = true;
        };
    }

    void GenerateLevel(CarGeneratorConfig config, CarGrid carGrid, System.Random random)
    {
        ICarGenerator carGenerator = new SimpleFullCarGenerator(this, config, carGrid, random);
        carGenerator.RegisterOnComplete(() =>
        {
            finishedGeneratingLevel = true;
        });
        carGenerator.Start();
    }

    IEnumerator Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Can only have one game instance!");
        }

        // TODO: do we want a seed?
        random = new System.Random();

        LoadConfigs();

        yield return new WaitUntil(() => finishedLoadingConfigs);

        // TODO: grab real car config
        CarGeneratorConfig config = new CarGeneratorConfig();
        currGrid = new CarGrid(config.width, config.height);

        // TODO: should we have a list of previous CarGrids, rather than just one current one?
        GenerateLevel(config, currGrid, random);
        Debug.Log("Generating level...");

        yield return new WaitUntil(() => finishedGeneratingLevel);
        Debug.Log("Level generation finished, starting simulation...");

        simulation = new Simulation(currGrid);
        Debug.Log(simulation.currentState);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            simulation.Step();
            Debug.Log(simulation.currentState);
        }
    }
}
