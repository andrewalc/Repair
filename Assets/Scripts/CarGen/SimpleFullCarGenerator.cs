using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SimpleFullCarGenerator : CoroutineCarGenerator
{
    private List<ICarGenerator> subGenerators;

    private System.Random random;

    public SimpleFullCarGenerator(MonoBehaviour host, CarGeneratorConfig config, CarGrid gridToUse, System.Random random) : base(host, config, gridToUse)
    {
        this.random = random;

        InitGenerators();
    }

    private void InitGenerators()
    {
        subGenerators = new List<ICarGenerator>();
        subGenerators.Add(new SimpleSpigotPlacer(Host, Config, ResultGrid, random));
        subGenerators.Add(new SimpleMachinePlacer(Host, Config, ResultGrid, random));
        subGenerators.Add(new SimpleObstaclePlacer(Host, Config, ResultGrid, random));
        subGenerators.Add(new DistanceBasedPlantPlacer(Host, Config, ResultGrid, random));
    }

    protected override IEnumerator PlaceObjects()
    {
        bool validLevel = false;
        while (!validLevel)
        {
            foreach (ICarGenerator generator in subGenerators)
            {
                bool generatorComplete = false;
                GenerationComplete eventHandler = () =>
                {
                    generatorComplete = true;
                };

                generator.RegisterOnComplete(eventHandler);

                generator.Start();
                yield return new WaitUntil(() => generatorComplete);

                generator.UnregisterOnComplete(eventHandler);
            }

            validLevel = CheckLevelConstraints(ResultGrid);
            if (!validLevel)
            {
                Debug.Log("Invalid level, re-generating.");
                ResultGrid.Clear();
            }
        }
    }

    private bool CheckLevelConstraints(CarGrid grid)
    {
        // Check if there is at least one plant.
        if (grid.SquaresEnumerable().Select((square) => square.ContainedObject.Type == CarObjectType.Plant).Count() == 0)
        {
            Debug.Log("Level does not have any plants.");
            return false;
        }

        return true;
    }

}