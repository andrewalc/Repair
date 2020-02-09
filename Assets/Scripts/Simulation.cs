using System;
using System.Collections.Generic;
using System.Linq;
using DarkConfig;
using UnityEngine;

public class Simulation {
    public CarGrid currentState;
    public SimulationSettingsConfig config;

    public int SimNum;

    // TODO: this should be a config val.
    public static int goodPlantHealth = 100;

	public event Action<GridSquare> plantSpawnEvent; 

    public Simulation(CarGrid grid, SimulationSettingsConfig config, int simNum) {
        currentState = grid.Clone();

        SimNum = simNum;

        for (int x = 0; x < currentState.Width; ++x) {
            for (int y = currentState.Height - 1; y >= 0; --y) {
                var contents = currentState.Squares[x, y].ContainedObject;
                switch (contents.Type) {
                    case CarObjectType.Plant: {
                        var plant = (PlantCarObject)contents;
                        plant.health = config.plantStartingHealth;
                        break;
                    }
                    case CarObjectType.Spigot:
                        break;
                    case CarObjectType.Machine: {
                        var machine = (MachineCarObject) contents;
                        machine.level = 1;
                    } break;
                    case CarObjectType.Empty:
                    case CarObjectType.Obstacle:
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        this.config = config;
    }

    public void Step() {
        var oldState = currentState.Clone();
        
        // ============= Update water levels =============
        currentState.waterLevel += config.baseWaterGenRate;
        for (int x = 0; x < oldState.Width; ++x) {
            for (int y = 0; y < oldState.Height; ++y) {
                if (oldState.Squares[x, y].ContainedObject.Type == CarObjectType.Machine) {
                    MachineCarObject machineObj = (MachineCarObject)oldState.Squares[x, y].ContainedObject;
                    if ( machineObj.MachineType == MachineCarObject.MachineTypes.Hydro)
                    {
                        currentState.waterLevel += config.hydroWaterGenRate * machineObj.level;
                    }
                    else
                    {
                        currentState.waterLevel += config.aeroWaterGenRate * machineObj.level;
                    }
                }
            }
        }
        // TODO sprinklers
        
        // ============= Update air quality =============
        for (int x = 0; x < oldState.Width; ++x) {
            for (int y = 0; y < oldState.Height; ++y) {
                var contained = oldState.Squares[x, y].ContainedObject; 
                if (contained.Type == CarObjectType.Machine) {
                    var machine = (MachineCarObject) contained;
                    if ( machine.MachineType == MachineCarObject.MachineTypes.Hydro)
                    {
                        currentState.airQuality -= (config.maxMachineLevel - machine.level) / (float) config.maxMachineLevel * config.hydroPollutionRate;
                    }
                    else
                    {
                        currentState.airQuality -= (config.maxMachineLevel - machine.level) / (float) config.maxMachineLevel * config.aeroPollutionRate;
                    }
                } else if (contained.Type == CarObjectType.Plant) {
                    currentState.airQuality += config.plantAQGenRate;
                }
            }
        }

        // ============= Update plants =============
        for (int x = 0; x < oldState.Width; ++x) {
            for (int y = oldState.Height - 1; y >= 0; --y) {
                switch (oldState.Squares[x, y].ContainedObject.Type) {
                    case CarObjectType.Empty:
                        // ============= Spawn new plants =============
                        foreach (var nearbyPlant in SurroundingObjects(oldState, x, y, c => c.Type == CarObjectType.Plant)) {
                            var plant = (PlantCarObject) nearbyPlant;

                            float chance = config.baseReproductionChance;
                            if (plant.health > 100) {
                                chance *= config.goodReproductionChanceCoefficient;
                            } else if (plant.health > 50) {
                                chance *= config.neutralReproductionChanceCoefficient;
                            } else {
                                chance *= config.badReproductionChanceCoefficient;
                            }

                            if (UnityEngine.Random.value <= chance) {
                                currentState.Squares[x, y].ContainedObject = new PlantCarObject();
								plantSpawnEvent?.Invoke(currentState.Squares[x, y]);
								break;
                            }
                        }
                        break;
                    case CarObjectType.Obstacle:
                        break;
                    case CarObjectType.Plant: {
                        var plant = (PlantCarObject) currentState.Squares[x, y].ContainedObject;
                        
                        // ============= Update plant health =============
                        var delta = oldState.IsWatered(x, y) ? config.lifeRate : -config.deathRate;
                        if (oldState.IsWatered(x, y) && oldState.airQuality <= config.badAQThreshold) {
                            delta *= config.badAQCoefficient;
                        }
                        
                        plant.health = Mathf.Clamp(plant.health + delta, 0, config.plantMaxHealth);

                        // ============= Generate plant matter =============

                        float deltaPlantMatter = config.basePMGenRate;
                        if (plant.health > 100) {
                            deltaPlantMatter *= config.goodPMCoefficient;
                        } else if (plant.health > 50) {
                            deltaPlantMatter *= config.neutralPMCoefficient;
                        } else {
                            deltaPlantMatter *= config.badPMCoefficient;
                        }
                        currentState.plantMatter += deltaPlantMatter;
                    } break;
                    case CarObjectType.Spigot:
                        break;
                    case CarObjectType.Machine:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        currentState.Sustainability = CalculateSustainability(currentState);
    }

    int NumSurroundingObjects(CarGrid state, int x, int y, CarObjectType type) {
        return SurroundingObjects(state, x, y, a => a.Type == type).Count();
    }

    IEnumerable<ICarObject> SurroundingObjects(CarGrid state, int x, int y, Func<ICarObject, bool> predicate) {
        if (x > 0 && predicate(state.Squares[x - 1, y].ContainedObject)) {
            yield return state.Squares[x - 1, y].ContainedObject;
        }

        if (x < state.Width - 1 && predicate(state.Squares[x + 1, y].ContainedObject)) {
            yield return state.Squares[x + 1, y].ContainedObject;
        }

        if (y > 0 && predicate(state.Squares[x, y - 1].ContainedObject)) {
            yield return state.Squares[x, y - 1].ContainedObject;
        }

        if (y < state.Height - 1 && predicate(state.Squares[x, y + 1].ContainedObject)) {
            yield return state.Squares[x, y + 1].ContainedObject;
        }
    }

    private float CalculateSustainability(CarGrid newState)
    {
        int numPlots = newState.SquaresEnumerable().Where((square) => !square.ContainedObject.BlocksIrrigation() && !square.ContainedObject.IsWaterSource())
                                                    .Count((square) => square.MinDistFromWaterSource < float.PositiveInfinity && square.MinDistFromInitialPlants < float.PositiveInfinity);

        int goodPlantCount = newState.SquaresEnumerable().Select((square) => square.ContainedObject).OfType<PlantCarObject>().Count(plant => plant.health > goodPlantHealth);

        int totalMachineLevels = newState.SquaresEnumerable().Select((square) => square.ContainedObject ).OfType<MachineCarObject>().Select((machine) => machine.level - 1).Sum();

        int machineCount = newState.SquaresEnumerable().Select((square) => square.ContainedObject ).OfType<MachineCarObject>().Count();

        int maxMachineLevel = Game.Instance.Simulation.config.maxMachineLevel - 1;

        float machineSustainability = (.6f * totalMachineLevels / (machineCount * maxMachineLevel));

        float plantSustainability = (.4f * goodPlantCount / numPlots);

        float sustainability = (float)(100*(machineSustainability + plantSustainability));

        return sustainability;
    }
}

