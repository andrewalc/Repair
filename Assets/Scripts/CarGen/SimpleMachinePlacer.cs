using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimpleMachinePlacer : SimpleCarObjectPlacer
{
    public SimpleMachinePlacer(MonoBehaviour host, CarGeneratorConfig config, CarGrid gridToUse, System.Random random) : base(host, config, gridToUse, random)
    {}

    protected override int GetNumObjectsToGenerate()
    {
        return Random.Next(Config.numMachinesMin, Config.numMachinesMax);
    }

    protected override ICarObject GenerateCarObject()
    {
        // TODO: Give the machine properties if necessary.
        return new MachineCarObject();
    }
}