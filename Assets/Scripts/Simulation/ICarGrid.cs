using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGrid
{
    public GridSquare[][] Squares
    {
        get;
        set;
    }

    public GridSquare GetSquare(int x, int y)
    {
        return Squares[x][y];
    }

    public void SetSquare(int x, int y, GridSquare square)
    {
        Squares[x][y] = square;
    }
}
