using System.Collections.Generic;

[ConfigMandatory]
public class SimulationSettingsConfig
{
    public float tickSpeed;
    public float plantStartingHealth;
    public float plantMaxHealth;

    /// How much the plant health changes every ticksBeforePlantHealthChanges ticks.
    /// Added if it's adequately watered, subtracted if it's not.
    public float plantWateredHealthGain;

    /// How many ticks between watered-state-driven health changes for a plant.
    public uint ticksBeforePlantHealthChanges;

    /// How much plant matter a plant produces as a multiple of its health.
    /// e.g. a plant with 120 health will produce 120 * 0.2 = 24 plant matter per tick.
    public float plantMatterHealthMultiplier = 0.2f;

    /// Chance that a plant will reproduce to an adjacent square in a tick as a multiple of its health.
    public float plantChanceOfReproducingMultiplier = 0.1f;
    
    public List<CarGrid> testGrids = new List<CarGrid>();
}
