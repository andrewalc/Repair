using System;

public class GridSquare {
    public int X { get; }
    public int Y { get; }
    public ICarObject ContainedObject { get; set; }

    private float minDistFromWaterSource;
    public float MinDistFromWaterSource {
        get => minDistFromWaterSource;
        set => minDistFromWaterSource = value < 0 ? float.PositiveInfinity : value;
    }

    private float minDistFromInitialPlants;
    public float MinDistFromInitialPlants {
        get => minDistFromInitialPlants;
        set => minDistFromInitialPlants = value < 0 ? float.PositiveInfinity : value;
    }

    public GridSquare(int x, int y) {
        X = x;
        Y = y;
        ContainedObject = new EmptyCarObject();
    }

    public GridSquare Clone() {
        return new GridSquare(X, Y) {
            ContainedObject = ContainedObject == null ? null : ContainedObject.Clone(),
            MinDistFromWaterSource = this.MinDistFromWaterSource,
            MinDistFromInitialPlants = this.MinDistFromInitialPlants
        };
    }
}
