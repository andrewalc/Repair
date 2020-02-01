using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        subGenerators.Add(new SimpleMachinePlacer(Host, Config, ResultGrid, random));
        subGenerators.Add(new SimpleObstaclePlacer(Host, Config, ResultGrid, random));
    }

    protected override IEnumerator PlaceObjects()
    {
        foreach( ICarGenerator generator in subGenerators)
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
    }

}