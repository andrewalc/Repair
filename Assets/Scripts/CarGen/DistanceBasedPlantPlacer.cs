using System;
using UnityEngine;

public class DistanceBasedPlantPlacer : DistanceBasedCarObjectPlacer
{
    public DistanceBasedPlantPlacer(MonoBehaviour host, CarGeneratorConfig config, CarGrid gridToUse, System.Random random) : base(host, config, gridToUse, random)
    {}

    protected override int GetNumObjectsToGenerate()
    {
        // Always generate at least one plant, but between min and max.
        return Math.Max(1, Random.Next(Config.numPlantsMin, Config.numPlantsMax));
    }

    protected override ICarObject GenerateCarObject()
    {
        return new PlantCarObject();
    }
}