public class GridSquare {
    public int X { get; }
    public int Y { get; }
    public ICarObject ContainedObject { get; set; }

    public GridSquare(int x, int y) {
        X = x;
        Y = y;
    }
}
