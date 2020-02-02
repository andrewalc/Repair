using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

public static class CarGridExtensions
{
    private static IEnumerable<Tuple<int, int>> neighborOffsets = new List<Tuple<int, int>>()
    {
        new Tuple<int,int>(-1, 0),

        new Tuple<int,int>(1, 0),

        new Tuple<int,int>(0, -1),

        new Tuple<int,int>(0, 1),
    };

    public static IEnumerable<GridSquare> SquaresEnumerable(this CarGrid carGrid)
    {
        return carGrid.Squares.OfType<GridSquare>();
    }

    public static void SetSquare(this CarGrid carGrid, GridSquare square)
    {
        carGrid.Squares[square.X, square.Y] = square;
    }

    public static int Width(this CarGrid grid)
    {
        return grid.Squares.GetLength(0);
    }
    
    public static int Height(this CarGrid grid)
    {
        return grid.Squares.GetLength(1);
    }

    public static Tuple<int, int> GetPos(this GridSquare square)
    {
        return new Tuple<int, int>(square.X, square.Y);
    }

    public static IEnumerable<Tuple<int,int>> SquarePositions(this CarGrid grid)
    {
        return grid.SquaresEnumerable().Select((square) => square.GetPos());
    }
    
    public static IEnumerable<GridSquare> GetNeighbors(this CarGrid grid, GridSquare square)
    {
        foreach ( Tuple<int, int> offset in neighborOffsets )
        {
            int x = square.X + offset.Item1;
            int y = square.Y + offset.Item2;
            if (x < 0 || x >= grid.Width())
            {
                continue;
            }

            if (y < 0 || y >= grid.Height())
            {
                continue;
            }

            yield return grid.Squares[x, y];
        }
    }
}