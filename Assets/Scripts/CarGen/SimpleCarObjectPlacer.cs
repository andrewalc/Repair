using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

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

        float startTime = Timer.ElapsedMilliseconds;
        Timer.Start();
        
        for ( int i = 0; i < numCarObjectsToGenerate; ++i)
        {
            IEnumerable<GridSquare> possibleSquares = ResultGrid.SquaresEnumerable().Where(
                                                            (square) => null == square.ContainedObject || square.ContainedObject.IsEmpty()
                                                        );

            if ( possibleSquares.Count() == 0)
            {
                break;
            }
            int selectedSquareIdx = Random.Next(possibleSquares.Count() - 1);
            GridSquare selectedSquare = possibleSquares.ElementAt(selectedSquareIdx);

            ICarObject carobject = GenerateCarObject();
            
            selectedSquare.ContainedObject = carobject;

            ResultGrid.SetSquare(selectedSquare);

            if (Timer.ElapsedMilliseconds - startTime > PerFrameBudget)
            {
                Timer.Stop();
                yield return null;
                startTime = Timer.ElapsedMilliseconds;
                Timer.Start();
            }
        }
    }

    protected abstract int GetNumObjectsToGenerate();

    protected abstract ICarObject GenerateCarObject();
}