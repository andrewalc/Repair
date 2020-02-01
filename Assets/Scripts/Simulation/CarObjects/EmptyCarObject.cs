public class EmptyCarObject : ICarObject {
    public CarObjectType Type => CarObjectType.Empty;
    
    public float GetPlantGrowthModifier() {
        throw new System.NotImplementedException();
    }

    public bool BlocksIrrigation() { return false; }
    public bool IsEmpty() { return true; }
    public bool IsWaterSource() { return false; }
    public ICarObject Clone() { return new EmptyCarObject(); }
}
