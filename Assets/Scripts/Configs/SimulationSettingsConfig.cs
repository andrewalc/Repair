using System.Collections.Generic;

[ConfigMandatory]
public class SimulationSettingsConfig {
    public Dictionary<string, CarGrid> testGrids = new Dictionary<string, CarGrid>();
}
