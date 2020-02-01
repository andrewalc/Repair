using System.Collections;
using System.Collections.Generic;

public class SimpleFullCarGenerator : CoroutineCarGenerator
{
    private List<ICarGenerator> subGenerators;

    private System.Random random;

    private ICarGenerator inProgressGenerator;

    private int nextGenerator;

    public SimpleFullCarGenerator(CarGeneratorConfig config, CarGrid gridToUse, System.Random random) : base(config, gridToUse)
    {
        this.random = random;

        InitGenerators();
    }

    private void InitGenerators()
    {
        subGenerators = new List<ICarGenerator>();
        subGenerators.Add(new SimpleMachinePlacer(Config, ResultGrid, random));
    }

    protected override IEnumerator PlaceObjects()
    {
        nextGenerator = 0;
        // TODO: maybe have a max run-time here, to avoid infinite loops
        while (true)
        {
            if ( null != inProgressGenerator )
            {
                // Wait for the generator to finish.
                yield return null;
            }

            if (nextGenerator >= subGenerators.Count)
            {
                // We are done.
                break;
            }

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