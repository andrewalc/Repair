using System.Collections.Generic;

[ConfigMandatory]
public class SimulationSettingsConfig
{
    public float tickSpeed;
    public float plantStartingHealth;
    public float plantMaxHealth;
    
    public float baseWaterGenRate;
    public float hydroWaterGenRate;
    public float aeroWaterGenRate;
    public float sprinklerUseRate;

    public int maxMachineLevel;

    // Air Quality
    public float hydroPollutionRate;
    public float aeroPollutionRate;
    public float plantAQGenRate;

    // Plant Material
    public float basePMGenRate;
    public float badPMCoefficient;
    public float neutralPMCoefficient;
    public float goodPMCoefficient;

    // Plant Health
    public float lifeRate;
    public float deathRate;
    public float badAQCoefficient;
    public float badAQThreshold;
    
    // Costs
    public float pipePrice;
    public float sprinklerPrice;
    public float avgReclaimCost;

    /// Chance that a plant will reproduce to an adjacent square in a tick as a multiple of its health.
    public float baseReproductionChance;
    public float goodReproductionChanceCoefficient;
    public float neutralReproductionChanceCoefficient;
    public float badReproductionChanceCoefficient;
    
    public List<CarGrid> testGrids = new List<CarGrid>();
}
