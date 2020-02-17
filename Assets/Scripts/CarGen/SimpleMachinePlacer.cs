using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimpleMachinePlacer : SimpleCarObjectPlacer
{
    public SimpleMachinePlacer(MonoBehaviour host, CarGenDifficultyLevelConfig config, CarGeneratorConfig basicConfig, CarGrid gridToUse, System.Random random) : base(host, config, basicConfig, gridToUse, random)
    {}

    protected override int GetNumObjectsToGenerate()
    {
        return Random.Next(Config.numMachinesMin, Config.numMachinesMax);
    }

    protected override ICarObject GenerateCarObject()
    {
        IEnumerable<MachineCarObject.MachineTypes> possibleMachineTypes = Enum.GetValues(typeof(MachineCarObject.MachineTypes)).OfType<MachineCarObject.MachineTypes>();
        
        int selectedTypeIdx = SelectMachineType(possibleMachineTypes);
        
        MachineCarObject machineObject = new MachineCarObject()
        {
            MachineType = possibleMachineTypes.ElementAt(selectedTypeIdx)
        };

        return machineObject;
    }

    private int SelectMachineType(IEnumerable<MachineCarObject.MachineTypes> possibleMachineTypes)
    {
        double totalProbability = Config.machineTypeProbabilities.Values.Sum();

        double randomVal = Random.NextDouble() * totalProbability;

        int typeIdx = 0;
        foreach(MachineCarObject.MachineTypes machineType in possibleMachineTypes)
        {
            randomVal -= Config.machineTypeProbabilities[machineType];
            if (randomVal > 0)
            {
                typeIdx++;
            }
            else
            {
                break;
            }
        }

        return typeIdx;
    }
}