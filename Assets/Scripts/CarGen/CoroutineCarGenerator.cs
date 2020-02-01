using System.Collections;
using UnityEngine;

public abstract class CoroutineCarGenerator : ICarGenerator
{
    private MonoBehaviour host;
    public MonoBehaviour Host
    { get{ return host; }}

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

    public delegate IEnumerator GenerationProcess();
    private GenerationProcess generationProcess;

    public CoroutineCarGenerator(MonoBehaviour host, CarGeneratorConfig config)
    {
        this.host = host;
        this.config = config;
    }

    public CoroutineCarGenerator(MonoBehaviour host, CarGeneratorConfig config, CarGrid gridToUse)
    {
        this.host = host;
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

        generationProcess = PlaceObjectsManager;
        host.StartCoroutine(generationProcess());
    }

    private IEnumerator PlaceObjectsManager()
    {
        yield return host.StartCoroutine(PlaceObjects());
        
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