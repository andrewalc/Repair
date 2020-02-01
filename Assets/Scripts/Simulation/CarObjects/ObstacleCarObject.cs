public class ObstacleCarObject : ICarObject {
    public CarObjectType Type => CarObjectType.Obstacle;

    public float GetPlantGrowthModifier() {
        throw new System.NotImplementedException();
    }

    public bool BlocksIrrigation() { return true; }
    public bool IsEmpty() { return false; }
    public bool IsWaterSource() { return false; }
}
