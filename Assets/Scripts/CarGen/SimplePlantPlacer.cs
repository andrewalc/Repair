using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimplePlantPlacer : SimpleCarObjectPlacer
{
    public SimplePlantPlacer(MonoBehaviour host, CarGenDifficultyLevelConfig config, CarGeneratorConfig basicConfig, CarGrid gridToUse, System.Random random) : base(host, config, basicConfig, gridToUse, random)
    {}

    protected override int GetNumObjectsToGenerate()
    {
        return Random.Next(Config.numPlantsMin, Config.numPlantsMax);
    }

    protected override ICarObject GenerateCarObject()
    {
        return new PlantCarObject();
    }
}