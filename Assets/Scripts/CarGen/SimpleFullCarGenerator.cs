using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFullCarGenerator : CoroutineCarGenerator
{
    private List<ICarGenerator> subGenerators;

    private System.Random random;

    private ICarGenerator inProgressGenerator;

    private int nextGenerator;

    public SimpleFullCarGenerator(MonoBehaviour host, CarGeneratorConfig config, CarGrid gridToUse, System.Random random) : base(host, config, gridToUse)
    {
        this.random = random;

        InitGenerators();
    }

    private void InitGenerators()
    {
        subGenerators = new List<ICarGenerator>();
        subGenerators.Add(new SimpleMachinePlacer(Host, Config, ResultGrid, random));
    }

    protected override IEnumerator PlaceObjects()
    {
        nextGenerator = 0;
        // TODO: do a wait until sub-generator instead of this loop stuff
        while( true )
        {
            // TODO: maybe have a max run-time here, to avoid infinite loops
            while (null != inProgressGenerator)
            {
                yield return null;
            }

            if (nextGenerator >= subGenerators.Count)
            {
                Debug.Log("No more generators.");
                // We are done.
                break;
            }

            Debug.Log("Generator " + nextGenerator + " finished.");
            // Start the next generator.
            inProgressGenerator = subGenerators[nextGenerator];
            inProgressGenerator.RegisterOnComplete(OnGeneratorComplete);
            ++nextGenerator;
            inProgressGenerator.Start();

            yield return null;
        }
    }

    private void OnGeneratorComplete()
    {
        inProgressGenerator.UnregisterOnComplete(OnGeneratorComplete);
        inProgressGenerator = null;
    }
}