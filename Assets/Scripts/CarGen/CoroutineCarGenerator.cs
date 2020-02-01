using System.Collections;
using UnityEngine;

public abstract class CoroutineCarGenerator : MonoBehaviour, ICarGenerator
{
    private GenerationComplete generationComplete;

    private CarGeneratorConfig config;
    protected CarGeneratorConfig Config { get { return config; } }

    private CarGrid resultGrid;

    protected CarGrid ResultGrid
    {
        get { return resultGrid; }
        set
        {
            if (resultGrid != null)
            {
                Debug.LogError("Result grid already set.");
            }
            resultGrid = value;
        }
    }

    private IEnumerator generationProcess;

    public CoroutineCarGenerator(CarGeneratorConfig config)
    {
        this.config = config;
    }

    public CoroutineCarGenerator(CarGeneratorConfig config, CarGrid gridToUse)
    {
        this.config = config;
        this.resultGrid = gridToUse;
    }

    public void Start()
    {
        if (null != generationProcess)
        {
            Debug.LogError("Tried to start a new generation while there was already one in progress.");
            return;
        }

        generationProcess = PlaceObjectsManager();
        StartCoroutine(generationProcess);
    }

    private IEnumerator PlaceObjectsManager()
    {
        yield return PlaceObjects();
        OnGenerationComplete();
    }

    protected abstract IEnumerator PlaceObjects();

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
        if (null != generationComplete)
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