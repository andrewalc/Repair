using System.Collections.Generic;
using System.Collections;
using System.Linq;

public static class CarGridExtensions
{
    public static IEnumerable<GridSquare> SquaresEnumerable(this CarGrid carGrid)
    {
        return carGrid.Squares.OfType<GridSquare>();
    }

    public static void SetSquare(this CarGrid carGrid, GridSquare square)
    {
        carGrid.Squares[square.X, square.Y] = square;
    }
}