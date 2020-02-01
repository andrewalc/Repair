public class CarGrid {
    public GridSquare[,] Squares;
    
    public CarGrid(int width, int height) {
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                Squares[x, y] = new GridSquare();
            }
        }
    }
}
