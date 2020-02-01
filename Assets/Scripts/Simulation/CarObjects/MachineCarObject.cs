public class MachineCarObject : ICarObject {
    public CarObjectType Type => CarObjectType.Machine;
    
    public float GetPlantGrowthModifier() {
        throw new System.NotImplementedException();
    }

    public bool BlocksIrrigation() { return true; }
    public bool IsEmpty() { return false; }
    public bool IsWaterSource() { return false; }
    public ICarObject Clone() { return new MachineCarObject(); }
}
