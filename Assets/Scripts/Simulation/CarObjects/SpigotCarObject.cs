public class SpigotCarObject : ICarObject {
    public CarObjectType Type => CarObjectType.Spigot;
    
    public float GetPlantGrowthModifier() {
        throw new System.NotImplementedException();
    }

    public bool BlocksIrrigation() { return false; }
    public bool IsEmpty() { return false; }
    public bool IsWaterSource() { return true; }
    public ICarObject Clone() { return new SpigotCarObject(); }
}
