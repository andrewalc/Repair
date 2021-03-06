using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimpleSpigotPlacer : SimpleCarObjectPlacer
{
    public SimpleSpigotPlacer(MonoBehaviour host, CarGenDifficultyLevelConfig config, CarGeneratorConfig basicConfig, CarGrid gridToUse, System.Random random) : base(host, config, basicConfig, gridToUse, random)
    {}

    protected override int GetNumObjectsToGenerate()
    {
        return Random.Next(Config.numSpigotsMin, Config.numSpigotsMax);
    }

    protected override ICarObject GenerateCarObject()
    {
        return new SpigotCarObject();
    }
}