public class GridSquare {
    public int X { get; }
    public int Y { get; }
    public ICarObject ContainedObject { get; set; }

    public GridSquare(int x, int y) {
        X = x;
        Y = y;
        ContainedObject = new EmptyCarObject();
    }

    public GridSquare Clone() {
        return new GridSquare(X, Y) {
            ContainedObject = ContainedObject == null ? null : ContainedObject.Clone()
        };
    }
}
