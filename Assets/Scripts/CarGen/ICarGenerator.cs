public delegate void GenerationComplete();

public interface ICarGenerator
{
    void Start();

    void RegisterOnComplete(GenerationComplete registrant);
    void UnregisterOnComplete(GenerationComplete registrant);

    CarGrid GetResult();
}