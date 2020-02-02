public class MachineCarObject : ICarObject {
    public enum MachineTypes
    {
        Aero,
        Hydro
    }

    public CarObjectType Type => CarObjectType.Machine;

    public MachineTypes MachineType { get; set; }

    public int level = 3;
    
    public float GetPlantGrowthModifier() {
        throw new System.NotImplementedException();
    }

    public bool BlocksIrrigation() { return true; }
    public bool IsEmpty() { return false; }
    public bool IsWaterSource() { return false; }

    public ICarObject Clone()
    {
        MachineCarObject machine = new MachineCarObject();
        machine.level = this.level;
        machine.MachineType = this.MachineType;
        return machine;
    }
}
