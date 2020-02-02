using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SustainabilityLevelDisplay : TextIntDisplay
{
    protected override int GetAmount()
    {
        CarGrid currentGrid = Game.Instance.Simulation.currentState;
        
        int numPlots = currentGrid.Width * currentGrid.Height;

        int goodPlantCount = currentGrid.SquaresEnumerable().Select((square) => square.ContainedObject ).OfType<PlantCarObject>().Where((plant) => plant.health >= 100).Count();
        return (int)(.4 * goodPlantCount / numPlots);
    }
}