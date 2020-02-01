using System.Collections;
using UnityEngine;

public class SimpleCarGenerator : MonoBehaviour, ICarGenerator
{
    private GenerationComplete generationComplete;

    private CarGeneratorConfig config;

    private CarGrid resultGrid;

    private IEnumerator generationProcess;

    public SimpleCarGenerator(CarGeneratorConfig config)
    {
        this.config = config;
    }

    public void Start()
    {
        if ( null != generationProcess )
        {
            Debug.LogError("Tried to start a new generation while there was already one in progress.");
            return;
        }

        generationProcess = PlaceObjects();
        StartCoroutine(generationProcess);
    }

    private IEnumerator PlaceObjects()
    {
        resultGrid = new CarGrid(config.width, config.height);

        // TODO: place objects
        yield return null;

        OnGenerationComplete();
    }

    public void RegisterOnComplete(GenerationComplete registrant)
    {
        generationComplete += registrant;
    }

    public void UnregisterOnComplete(GenerationComplete registrant)
    {
        generationComplete -= registrant;
    }

    private void OnGenerationComplete()
    {
        if ( null != generationComplete )
        {
            generationComplete();
        }

        // We are done generating, clear the process.
        generationProcess = null;
    }

    public CarGrid GetResult()
    {
        return resultGrid;
    }
}