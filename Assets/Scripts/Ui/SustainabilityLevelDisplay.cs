using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SustainabilityLevelDisplay : TextIntDisplay
{
    // TODO: This should be a setting.
    public static int goodPlantHealth = 100;

    protected override int GetAmount()
    {
        CarGrid currentGrid = Game.Instance.Simulation.currentState;
        
        int numPlots = currentGrid.SquaresEnumerable().Select((square) => square.ContainedObject ).Where((obj) => !obj.BlocksIrrigation()).Count();

        int goodPlantCount = currentGrid.SquaresEnumerable().Select((square) => square.ContainedObject ).OfType<PlantCarObject>().Where((plant) => plant.health > goodPlantHealth).Count();
        
        int totalMachineLevels = currentGrid.SquaresEnumerable().Select((square) => square.ContainedObject ).OfType<MachineCarObject>().Select((machine) => machine.level).Sum();
        
        int machineCount = currentGrid.SquaresEnumerable().Select((square) => square.ContainedObject ).OfType<MachineCarObject>().Count();

        int maxMachineLevel = Game.Instance.Simulation.config.maxMachineLevel;

        return (int)(100*(.6 * totalMachineLevels / (machineCount * maxMachineLevel)) + (.4 * goodPlantCount / numPlots));
    }
}