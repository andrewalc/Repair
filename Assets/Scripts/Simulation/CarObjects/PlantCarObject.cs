public class PlantCarObject : ICarObject {
    public CarObjectType Type => CarObjectType.Plant;
    
    public float GetPlantGrowthModifier() {
        throw new System.NotImplementedException();
    }

    public bool BlocksIrrigation() { return false; }
    public bool IsEmpty() { return false; }
    public bool IsWaterSource() { return false; }
    public ICarObject Clone() { return new PlantCarObject(); }
}
