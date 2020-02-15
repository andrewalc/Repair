using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Simulation
{
    public CarGrid currentState;
    public SimulationSettingsConfig config;

    public int SimNum;

    // TODO: this should be a config val.
    public static int goodPlantHealth = 100;

    public event Action<Simulation> OnSimTickFinished;

    public event Action<GridSquare> PlantSpawnEvent;

    public delegate void ResourceChangedEvent(float oldValue, ResourceEntry newEntry);
    public event ResourceChangedEvent ResourceChanged;

    public Simulation(CarGrid grid, SimulationSettingsConfig config, int simNum)
    {
        currentState = grid.Clone();
        currentState.ResourceChanged += OnResourceChanged;

        SimNum = simNum;

        for (int x = 0; x < currentState.Width; ++x)
        {
            for (int y = currentState.Height - 1; y >= 0; --y)
            {
                var contents = currentState.Squares[x, y].ContainedObject;
                switch (contents.Type)
                {
                    case CarObjectType.Plant:
                    {
                        var plant = (PlantCarObject) contents;
                        plant.health = config.plantStartingHealth;
                        break;
                    }
                    case CarObjectType.Spigot:
                        break;
                    case CarObjectType.Machine:
                    {
                        var machine = (MachineCarObject) contents;
                        machine.level = 1;
                    }
                        break;
                    case CarObjectType.Empty:
                    case CarObjectType.Obstacle:
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        this.config = config;
    }

    private void OnResourceChanged(float oldValue, ResourceEntry resource)
    {
        ResourceChanged?.Invoke(oldValue, resource);
    }

    public void Step()
    {
        var oldState = currentState.Clone();

        // ============= Update water levels =============
        float waterLevelChange = config.baseWaterGenRate;
        for (int x = 0; x < oldState.Width; ++x)
        {
            for (int y = 0; y < oldState.Height; ++y)
            {
                if (oldState.Squares[x, y].ContainedObject.Type == CarObjectType.Machine)
                {
                    MachineCarObject machineObj = (MachineCarObject) oldState.Squares[x, y].ContainedObject;
                    if (machineObj.MachineType == MachineCarObject.MachineTypes.Hydro)
                    {
                        waterLevelChange += config.hydroWaterGenRate * machineObj.level;
                    }
                    else
                    {
                        waterLevelChange += config.aeroWaterGenRate * machineObj.level;
                    }
                }
            }
        }

        currentState.ChangeResource(ResourceType.Water, waterLevelChange);

        // ============= Update air quality =============
        float airQualityChange = 0.0f;
        for (int x = 0; x < oldState.Width; ++x)
        {
            for (int y = 0; y < oldState.Height; ++y)
            {
                var contained = oldState.Squares[x, y].ContainedObject;
                if (contained.Type == CarObjectType.Machine)
                {
                    var machine = (MachineCarObject) contained;
                    if (machine.MachineType == MachineCarObject.MachineTypes.Hydro)
                    {
                        airQualityChange -= (config.maxMachineLevel - machine.level) * config.hydroPollutionRate;
                    }
                    else
                    {
                        airQualityChange -= (config.maxMachineLevel - machine.level) * config.aeroPollutionRate;
                    }
                }
                else if (contained.Type == CarObjectType.Plant)
                {
                    airQualityChange += config.plantAQGenRate;
                }
            }
        }

        currentState.ChangeResource(ResourceType.AirQuality, airQualityChange);

        float waterLevel = currentState.GetResourceValue(ResourceType.Water);

        float plantMatterChange = 0;

        // ============= Update plants =============
        foreach (GridSquare square in oldState.SquaresEnumerable()
            .OrderBy((sq) => oldState.GetWaterTravelDist(sq.X, sq.Y)))
        {
            int x = square.X;
            int y = square.Y;
            switch (square.ContainedObject.Type)
            {
                case CarObjectType.Empty:
                    // ============= Spawn new plants =============
                    foreach (var nearbyPlant in SurroundingObjects(oldState, x, y, c => c.Type == CarObjectType.Plant))
                    {
                        var plant = (PlantCarObject) nearbyPlant;

                        float chance = config.baseReproductionChance;
                        if (plant.health > 100)
                        {
                            chance *= config.goodReproductionChanceCoefficient;
                        }
                        else if (plant.health > 50)
                        {
                            chance *= config.neutralReproductionChanceCoefficient;
                        }
                        else
                        {
                            chance *= config.badReproductionChanceCoefficient;
                        }

                        if (Random.value <= chance)
                        {
                            currentState.Squares[x, y].ContainedObject = new PlantCarObject();
                            PlantSpawnEvent?.Invoke(currentState.Squares[x, y]);
                            break;
                        }
                    }

                    break;
                case CarObjectType.Obstacle:
                    break;
                case CarObjectType.Plant:
                {
                    var plant = (PlantCarObject) currentState.Squares[x, y].ContainedObject;

                    // ============ Spend water ===============

                    bool isWatered = oldState.IsWatered(x, y);
                    if (isWatered)
                    {
                        if (waterLevel <= 0.0f)
                        {
                            // If there is no water left, we're not watered.
                            isWatered = false;
                        }

                        if (waterLevel < config.sprinklerUseRate)
                        {
                            waterLevel = 0;
                        }
                        else
                        {
                            waterLevel -= config.sprinklerUseRate;
                        }
                    }

                    // ============= Update plant health =============

                    var delta = isWatered ? config.lifeRate : -config.deathRate;
                    if (isWatered && oldState.GetResourceValue(ResourceType.AirQuality) <= config.badAQThreshold)
                    {
                        delta *= config.badAQCoefficient;
                    }

                    plant.health = Mathf.Clamp(plant.health + delta, 0, config.plantMaxHealth);

                    // ============= Generate plant matter =============

                    float deltaPlantMatter = config.basePMGenRate;
                    if (plant.health > 100)
                    {
                        deltaPlantMatter *= config.goodPMCoefficient;
                    }
                    else if (plant.health > 50)
                    {
                        deltaPlantMatter *= config.neutralPMCoefficient;
                    }
                    else
                    {
                        deltaPlantMatter *= config.badPMCoefficient;
                    }

                    plantMatterChange += deltaPlantMatter;
                }
                    break;
                case CarObjectType.Spigot:
                    break;
                case CarObjectType.Machine:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        currentState.SetResource(ResourceType.Water, waterLevel);
        currentState.ChangeResource(ResourceType.PlantMatter, plantMatterChange);

        currentState.Sustainability = CalculateSustainability(currentState);

        OnSimTickFinished?.Invoke(this);
    }

    int NumSurroundingObjects(CarGrid state, int x, int y, CarObjectType type)
    {
        return SurroundingObjects(state, x, y, a => a.Type == type).Count();
    }

    IEnumerable<ICarObject> SurroundingObjects(CarGrid state, int x, int y, Func<ICarObject, bool> predicate)
    {
        if (x > 0 && predicate(state.Squares[x - 1, y].ContainedObject))
        {
            yield return state.Squares[x - 1, y].ContainedObject;
        }

        if (x < state.Width - 1 && predicate(state.Squares[x + 1, y].ContainedObject))
        {
            yield return state.Squares[x + 1, y].ContainedObject;
        }

        if (y > 0 && predicate(state.Squares[x, y - 1].ContainedObject))
        {
            yield return state.Squares[x, y - 1].ContainedObject;
        }

        if (y < state.Height - 1 && predicate(state.Squares[x, y + 1].ContainedObject))
        {
            yield return state.Squares[x, y + 1].ContainedObject;
        }
    }

    private float CalculateSustainability(CarGrid newState)
    {
        int numPlots = newState.CalculatePossiblePlantPlots();
            
        int goodPlantCount = newState.SquaresEnumerable().Select((square) => square.ContainedObject)
            .OfType<PlantCarObject>().Count(plant => plant.health > goodPlantHealth);

        int totalMachineLevels = newState.SquaresEnumerable().Select((square) => square.ContainedObject)
            .OfType<MachineCarObject>().Select((machine) => machine.level - 1).Sum();

        int machineCount = newState.SquaresEnumerable().Select((square) => square.ContainedObject)
            .OfType<MachineCarObject>().Count();

        int maxMachineLevel = Game.Instance.Simulation.config.maxMachineLevel - 1;

        float machineSustainability = (.6f * totalMachineLevels / (machineCount * maxMachineLevel));

        float plantSustainability = (.4f * goodPlantCount / numPlots);

        float sustainability = (float) (100 * (machineSustainability + plantSustainability));

        return sustainability;
    }
}