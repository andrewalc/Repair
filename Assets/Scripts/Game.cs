using System;
using System.Collections;
using DarkConfig;
using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }

    public bool finishedLoadingConfigs { get; private set; }

    public bool finishedGeneratingLevel { get; private set; }

    private System.Random random;

    public SimulationSettingsConfig SimulationSettings = new SimulationSettingsConfig();

    private CarGeneratorConfig CarGenConfig = new CarGeneratorConfig();

    public GameObject IrrigationPanelPrefab;
    GameObject IrrigationPanelInstance;

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
        get
        {
            return carGrids[CurrCarNum];
        }
    }

    public int CurrCarNum { get; private set; }

    private List<Simulation> carSims = new List<Simulation>();
    public IEnumerable<Simulation> CarSims
    {
        get
        {
            return carSims;
        }
    }

    private List<CarGrid> carGrids = new List<CarGrid>();
    public IEnumerable<CarGrid> CarGrids
    {
        get
        {
            return carGrids;
        }
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
            Config.Apply("carGeneratorSettings", ref CarGenConfig);
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
        finishedGeneratingLevel = false;

        if (Simulation != null)
        {
            LevelEnded?.Invoke(Simulation);
            Simulation.OnSimTickFinished -= SendSimTickFinished;
            Simulation.ResourceChanged -= OnResourceChanged;
        }

        CarGrid newGrid = new CarGrid(CarGenConfig.width, CarGenConfig.height);

        GenerateLevel(CarGenConfig, newGrid, random);
        Debug.Log("Generating level...");

        yield return new WaitUntil(() => finishedGeneratingLevel);
        Debug.Log("Level generation finished, starting simulation...");

        Simulation newSim = new Simulation(newGrid, SimulationSettings, CurrCarNum + 1);
        Debug.Log(newSim.currentState);

        carSims.Add(newSim);
        carGrids.Add(newGrid);
        if (CurrCarNum >= 0) {
            Tick.Instance.RemoveEventListener(carSims[CurrCarNum].Step);   
        }
        CurrCarNum++;
        Tick.Instance.AddEventListener(newSim.Step);

        newSim.OnSimTickFinished += SendSimTickFinished;
        newSim.ResourceChanged += OnResourceChanged;
        
        LevelGenerated?.Invoke(newSim);
        Debug.Log("Current sim num: " + carSims.Count + " curr car num: " + CurrCarNum);
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
}
