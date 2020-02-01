using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SimpleCarObjectPlacer : CoroutineCarGenerator
{
    protected System.Random Random
    {
        get;
        private set;
    }

    public SimpleCarObjectPlacer(MonoBehaviour host, CarGeneratorConfig config, CarGrid gridToUse, System.Random random) : base(host, config, gridToUse)
    {
        Random = random;
    }

    protected override IEnumerator PlaceObjects()
    {
        int numCarObjectsToGenerate = GetNumObjectsToGenerate();

        for ( int i = 0; i < numCarObjectsToGenerate; ++i)
        {
            IEnumerable<GridSquare> possibleSquares = ResultGrid.SquaresEnumerable().Where(
                                                            (square) => null == square.ContainedObject || square.ContainedObject.IsEmpty()
                                                        );

            int selectedSquareIdx = Random.Next(possibleSquares.Count() - 1);
            GridSquare selectedSquare = possibleSquares.ElementAt(selectedSquareIdx);

            ICarObject carobject = GenerateCarObject();
            
            selectedSquare.ContainedObject = carobject;

            ResultGrid.SetSquare(selectedSquare);

            yield return null;
        }
    }

    protected abstract int GetNumObjectsToGenerate();

    protected abstract ICarObject GenerateCarObject();
}