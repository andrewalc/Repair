using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Simulation {
    public CarGrid currentState;
    public SimulationSettingsConfig config;

    public Simulation(CarGrid grid, SimulationSettingsConfig config) {
        currentState = grid.Clone();

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
                        machine.level = Mathf.RoundToInt(config.maxMachineLevel / 2f);
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
        var newState = currentState.Clone();
        
        // ============= Update water levels =============
        newState.waterLevel += config.baseWaterGenRate;
        for (int x = 0; x < currentState.Width; ++x) {
            for (int y = 0; y < currentState.Height; ++y) {
                if (currentState.Squares[x, y].ContainedObject.Type == CarObjectType.Machine) {
                    newState.waterLevel += config.hydroWaterGenRate;
                    // TODO aero machines
                }
            }
        }
        // TODO sprinklers
        
        // ============= Update air quality =============
        for (int x = 0; x < currentState.Width; ++x) {
            for (int y = 0; y < currentState.Height; ++y) {
                var contained = currentState.Squares[x, y].ContainedObject; 
                if (contained.Type == CarObjectType.Machine) {
                    var machine = (MachineCarObject) contained;
                    newState.airQuality -= (config.maxMachineLevel - machine.level) / (float) config.maxMachineLevel * config.hydroPollutionRate;
                } else if (contained.Type == CarObjectType.Plant) {
                    newState.airQuality += config.plantAQGenRate;
                }
            }
        }

        // ============= Update plants =============
        for (int x = 0; x < currentState.Width; ++x) {
            for (int y = currentState.Height - 1; y >= 0; --y) {
                switch (currentState.Squares[x, y].ContainedObject.Type) {
                    case CarObjectType.Empty:
                        // ============= Spawn new plants =============
                        foreach (var nearbyPlant in SurroundingObjects(x, y, c => c.Type == CarObjectType.Plant)) {
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
                                newState.Squares[x, y].ContainedObject = new PlantCarObject();
                                break;
                            }
                        }
                        break;
                    case CarObjectType.Obstacle:
                        break;
                    case CarObjectType.Plant: {
                        var plant = (PlantCarObject) newState.Squares[x, y].ContainedObject;
                        
                        // ============= Update plant health =============
                        var delta = currentState.IsWatered(x, y) ? config.lifeRate : -config.deathRate;
                        if (currentState.IsWatered(x, y) && currentState.airQuality <= config.badAQThreshold) {
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
                        newState.plantMatter += deltaPlantMatter;
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

        currentState = newState;
    }

    int NumSurroundingObjects(int x, int y, CarObjectType type) {
        return SurroundingObjects(x, y, a => a.Type == type).Count();
    }

    IEnumerable<ICarObject> SurroundingObjects(int x, int y, Func<ICarObject, bool> predicate) {
        if (x > 0 && predicate(currentState.Squares[x - 1, y].ContainedObject)) {
            yield return currentState.Squares[x - 1, y].ContainedObject;
        }

        if (x < currentState.Width - 1 && predicate(currentState.Squares[x + 1, y].ContainedObject)) {
            yield return currentState.Squares[x + 1, y].ContainedObject;
        }

        if (y > 0 && predicate(currentState.Squares[x, y - 1].ContainedObject)) {
            yield return currentState.Squares[x, y - 1].ContainedObject;
        }

        if (y < currentState.Height - 1 && predicate(currentState.Squares[x, y + 1].ContainedObject)) {
            yield return currentState.Squares[x, y + 1].ContainedObject;
        }
    }
}

