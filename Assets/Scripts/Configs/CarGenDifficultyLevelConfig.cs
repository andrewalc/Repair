using System.Collections.Generic;

public class CarGenDifficultyLevelConfig
{
    // Number of levels to stay at this difficulty... -1 means stay here forever.
    public int numLevelsAtDifficulty = 1;
    
    public int width = 12;
    public int height = 6;

    public int numMachinesMin = 10;
    public int numMachinesMax = 10;

    public Dictionary<MachineCarObject.MachineTypes, double> machineTypeProbabilities;

    public int numObstaclesMin = 10;
    public int numObstaclesMax = 10;

    public int numPlantsMin = 5;
    public int numPlantsMax = 5;

    public float plantDistanceDifficultyRatioMin = 0.5f;
    public float plantDistanceDifficultyRatioMax = 0.7f;


    public int numSpigotsMin = 1;
    public int numSpigotsMax = 1;
    
    public int minAvailablePlantPlots = 10;
    public int maxAvailablePlantPlots = 30;
}