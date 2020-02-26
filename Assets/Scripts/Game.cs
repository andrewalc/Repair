﻿using System;
using System.Collections;
using DarkConfig;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public bool finishedLoadingConfigs { get; private set; }

    private bool finishedGeneratingLevel = false;
    public bool FinishedGeneratingLevel {
        get { return finishedGeneratingLevel && Simulation != null; }
        private set { finishedGeneratingLevel = value; }
    }

    private System.Random random;

    public SimulationSettingsConfig SimulationSettings = new SimulationSettingsConfig();

    private int currDifficulty = 0;
    private int numLevelsBeforeThisDifficulty = 0;
    private CarGeneratorConfig carGenConfig = new CarGeneratorConfig();

    private CarGenDifficultyLevelConfig CarGenDifficultyConfig
    {
        get { return carGenConfig.difficulties[currDifficulty]; }
    }

    [SerializeField] private TrainGenerator trainCarGenerator;

    public Simulation Simulation
    {
        get
        {
            if (CurrCarNum < 0 || carSims.Count <= CurrCarNum)
            {
                return null;
            }

            return carSims[CurrCarNum];
        }
    }

    public CarGrid CurrGrid
    {
        get { return Simulation.currentState; }
    }

    public int CurrCarNum { get; private set; }

    private List<Simulation> carSims = new List<Simulation>();

    public IEnumerable<Simulation> CarSims
    {
        get { return carSims; }
    }

    private List<CarGrid> carGrids = new List<CarGrid>();

    public IEnumerable<CarGrid> CarGrids
    {
        get { return carGrids; }
    }

    public delegate void OnLevelGenerated(Simulation newLevel);

    public event OnLevelGenerated LevelGenerated;

    public delegate void OnLevelEnded(Simulation newLevel);

    public event OnLevelEnded LevelEnded;

    public delegate void OnGameLoaded();

    public event OnGameLoaded GameLoaded;

    public delegate void OnBeginPlay();

    public event OnBeginPlay BeginPlay;

    public delegate void ResourceChangedEvent(Simulation sim, float oldValue, ResourceEntry newValue);

    public event ResourceChangedEvent ResourceChanged;

    public event Action<Simulation> OnSimTickFinished;

    public void BeginGame()
    {
        Tick.Instance.UnPause();

        BeginPlay?.Invoke();
    }

    void LoadConfigs()
    {
        UnityPlatform.Setup();
        Config.FileManager.AddSource(new ResourcesSource(hotload: true));
        Config.Preload();
        Config.OnPreload += () =>
        {
            Config.Apply("simulationSettings", ref SimulationSettings);
            Config.Apply("carGeneratorSettings", ref carGenConfig);
            finishedLoadingConfigs = true;
        };
    }

    void GenerateLevel(CarGenDifficultyLevelConfig config, CarGeneratorConfig basicConfig, CarGrid carGrid,
        System.Random random)
    {
        ICarGenerator carGenerator = new SimpleFullCarGenerator(this, config, basicConfig, carGrid, random);
        carGenerator.RegisterOnComplete(() => { FinishedGeneratingLevel = true; });
        carGenerator.Start();
        SoundManager.Instance.PlaySound(SoundNames.chooChoo);
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Can only have one game instance!");
        }
    }

    IEnumerator Start()
    {
        // TODO: do we want a seed?
        random = new System.Random();

        LoadConfigs();

        yield return new WaitUntil(() => finishedLoadingConfigs);

        CurrCarNum = -1;

        GameLoaded?.Invoke();
    }

    private IEnumerator GenerateNewCarInternal()
    {
        FinishedGeneratingLevel = false;

        if (Simulation != null)
        {
            LevelEnded?.Invoke(Simulation);
            Simulation.OnSimTickFinished -= SendSimTickFinished;
            Simulation.ResourceChanged -= OnResourceChanged;
        }

        UpdateDifficulty();

        CarGrid newGrid = new CarGrid(CarGenDifficultyConfig.width, CarGenDifficultyConfig.height);

        GenerateLevel(CarGenDifficultyConfig, carGenConfig, newGrid, random);
        Debug.Log("Generating level...");

        yield return new WaitUntil(() => finishedGeneratingLevel);
        Debug.Log("Level generation finished, starting simulation...");

        Simulation newSim = new Simulation(newGrid, SimulationSettings, CurrCarNum + 1);
        Debug.Log(newSim.currentState);

        carSims.Add(newSim);
        carGrids.Add(newGrid);
        if (CurrCarNum >= 0)
        {
            Tick.Instance.RemoveEventListener(carSims[CurrCarNum].Step);
        }

        CurrCarNum++;
        Tick.Instance.AddEventListener(newSim.Step);

        newSim.OnSimTickFinished += SimTicked;
        newSim.ResourceChanged += OnResourceChanged;

        LevelGenerated?.Invoke(newSim);
        Debug.Log("Current sim num: " + carSims.Count + " curr car num: " + CurrCarNum);
    }

    private void UpdateDifficulty()
    {
        // Consider moving to the next difficulty.
        // If negative levels at this difficulty, we stay at this difficulty forever. Or if it's the last.
        while (CarGenDifficultyConfig.numLevelsAtDifficulty >= 0 &&
               currDifficulty < carGenConfig.difficulties.Length - 1 &&
               CarGrids.Count() - numLevelsBeforeThisDifficulty >= CarGenDifficultyConfig.numLevelsAtDifficulty)
        {
            numLevelsBeforeThisDifficulty += CarGenDifficultyConfig.numLevelsAtDifficulty;
            currDifficulty++;
        }
    }

    private void SendSimTickFinished(Simulation sim)
    {
        OnSimTickFinished?.Invoke(sim);
    }

    private void OnResourceChanged(float oldValue, ResourceEntry newValue)
    {
        ResourceChanged?.Invoke(Simulation, oldValue, newValue);
    }

    public void GenerateNewCar()
    {
        StartCoroutine(GenerateNewCarInternal());
    }

    private void SimTicked(Simulation sim)
    {
        CheckLevelState(sim);
        SendSimTickFinished(sim);
    }

    public void CheckLevelState(Simulation sim)
    {
        if (!FinishedGeneratingLevel)
        {
            return;
        }

// If we have reached max sustainability, we have won! Move to the next level.
        if (sim.currentState.Sustainability >= 99)
        {
            SoundManager.Instance.PlaySound(SoundNames.win);

            // TODO: We should show a win screen here, and wait for it to close before generating the next car.

            trainCarGenerator.CreateTrainCar();
            FindObjectOfType<CameraShift>().AnimateForward();
        }
    }
}