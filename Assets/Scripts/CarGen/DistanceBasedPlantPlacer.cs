using System;
using UnityEngine;

public class DistanceBasedPlantPlacer : DistanceBasedCarObjectPlacer
{
    public DistanceBasedPlantPlacer(MonoBehaviour host, CarGenDifficultyLevelConfig config, CarGeneratorConfig basicConfig, CarGrid gridToUse, System.Random random) : base(host, config, basicConfig, gridToUse, random)
    {}

    protected override int GetNumObjectsToGenerate()
    {
        // Always generate at least one plant, but between min and max.
        return Math.Max(1, Random.Next(Config.numPlantsMin, Config.numPlantsMax));
    }

    protected override ICarObject GenerateCarObject()
    {
        PlantCarObject newPlant = new PlantCarObject();
        newPlant.health = Game.Instance.SimulationSettings.plantStartingHealth;
        return newPlant;
    }
}