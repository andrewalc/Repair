using System;

public class PlantCarObject : ICarObject {
    public CarObjectType Type => CarObjectType.Plant;
    
    public float health = 100;

    public float GetPlantGrowthModifier() {
        throw new NotImplementedException();
    }

    public bool BlocksIrrigation() { return false; }
    public bool IsEmpty() { return false; }
    public bool IsWaterSource() { return false; }
    public ICarObject Clone() { return (PlantCarObject)MemberwiseClone(); }
}
