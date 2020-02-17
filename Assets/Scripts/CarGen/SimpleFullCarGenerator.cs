using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SimpleFullCarGenerator : CoroutineCarGenerator
{
    private List<ICarGenerator> subGenerators;

    private System.Random random;

    public SimpleFullCarGenerator(MonoBehaviour host, CarGenDifficultyLevelConfig config, CarGeneratorConfig basicConfig, CarGrid gridToUse,
        System.Random random) : base(host, config, basicConfig, gridToUse)
    {
        this.random = random;

        InitGenerators();
    }

    private void InitGenerators()
    {
        subGenerators = new List<ICarGenerator>();
        subGenerators.Add(new SimpleSpigotPlacer(Host, Config, BasicConfig, ResultGrid, random));
        subGenerators.Add(new SimpleMachinePlacer(Host, Config, BasicConfig, ResultGrid, random));
        subGenerators.Add(new SimpleObstaclePlacer(Host, Config, BasicConfig, ResultGrid, random));
        subGenerators.Add(new DistanceBasedPlantPlacer(Host, Config, BasicConfig, ResultGrid, random));
    }

    protected override IEnumerator PlaceObjects()
    {
        bool validLevel = false;
        int attempts = 0;

        float startTime = Timer.ElapsedMilliseconds;
        Timer.Start();

        while (!validLevel && attempts < BasicConfig.maxLevelGenAttempts)
        {
            ++attempts;
            foreach (ICarGenerator generator in subGenerators)
            {
                bool generatorComplete = false;
                GenerationComplete eventHandler = () => { generatorComplete = true; };

                generator.RegisterOnComplete(eventHandler);

                generator.Start();
                WaitUntil generatorWait = new WaitUntil(() => generatorComplete);
                while (generatorWait.MoveNext())
                {
                    if (Timer.ElapsedMilliseconds - startTime > PerFrameBudget)
                    {
                        Timer.Stop();
                        yield return null;
                        startTime = Timer.ElapsedMilliseconds;
                        Timer.Start();
                    }
                }

                generator.UnregisterOnComplete(eventHandler);
            }

            // Init remaining calculated values in CarGrid.
            ResultGrid.RecalculateExtraInfo();

            if (attempts >= BasicConfig.maxLevelGenAttempts)
            {
                Debug.Log("Max level gen attempts reached, level constraints ignored.");
                break;
            }

            validLevel = CheckLevelConstraints(ResultGrid);
            if (!validLevel)
            {
                //Debug.Log("Invalid level, re-generating.");
                ResultGrid.Clear();
            }
            else
            {
                break;
            }

            if (Timer.ElapsedMilliseconds - startTime > PerFrameBudget)
            {
                Timer.Stop();
                yield return null;
                startTime = Timer.ElapsedMilliseconds;
                Timer.Start();
            }
        }
    }

    private bool CheckLevelConstraints(CarGrid grid)
    {
        // Check if there is at least one plant.
        if (!grid.SquaresEnumerable().Select((square) => square.ContainedObject.Type == CarObjectType.Plant).Any())
        {
            //Debug.Log("Level does not have any plants.");
            return false;
        }

        int possiblePlantPlots = grid.CalculatePossiblePlantPlots();
        if (possiblePlantPlots < Config.minAvailablePlantPlots)
        {
            //Debug.Log("Level does not have enough eventual plant plots.");
            return false;
        }

        if (possiblePlantPlots > Config.maxAvailablePlantPlots)
        {
            //Debug.Log("Level has too many eventual plant plots.");
            return false;
        }


        return true;
    }
}