public class SpigotCarObject : ICarObject {
    public CarObjectType Type => CarObjectType.Spigot;
    
    public float GetPlantGrowthModifier() {
        throw new System.NotImplementedException();
    }

    public bool BlocksIrrigation() { return true; }
    public bool IsEmpty() { return false; }
    public bool IsWaterSource() { return false; }
}
