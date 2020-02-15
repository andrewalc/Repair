using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class DistanceBasedCarObjectPlacer : CoroutineCarGenerator
{
    protected System.Random Random
    {
        get;
        private set;
    }

    public DistanceBasedCarObjectPlacer(MonoBehaviour host, CarGeneratorConfig config, CarGrid gridToUse, System.Random random) : base(host, config, gridToUse)
    {
        Random = random;
    }

    protected override IEnumerator PlaceObjects()
    {
        int numCarObjectsToGenerate = GetNumObjectsToGenerate();

        float startTime = Timer.ElapsedMilliseconds;
        Timer.Start();
        
        for (int i = 0; i < numCarObjectsToGenerate; ++i)
        {
            IEnumerable<GridSquare> possibleSquares = ResultGrid.SquaresEnumerable().Where(
                                                            (square) => null == square.ContainedObject || square.ContainedObject.IsEmpty()
                                                        );

            float[,] squareDists = new float[ResultGrid.Width(), ResultGrid.Height()];
            for (int x = 0; x < squareDists.GetLength(0); ++x)
            {
                for (int y = 0; y < squareDists.GetLength(1); ++y)
                {
                    squareDists[x,y] = float.PositiveInfinity;
                }
            }

            IEnumerable<GridSquare> spigots = ResultGrid.SquaresEnumerable().Where((square) => square.ContainedObject.IsWaterSource());
            foreach (GridSquare spigot in spigots)
            {
                float[,] spigotDists = CalculateDistances(ResultGrid, spigot);
                for (int x = 0; x < squareDists.GetLength(0); ++x)
                {
                    for (int y = 0; y < squareDists.GetLength(1); ++y)
                    {
                        squareDists[x, y] = Math.Min(squareDists[x, y], spigotDists[x, y]);
                    }
                }
            }

            IEnumerable<GridSquare> sortedPossibleSquares = possibleSquares.Where((square) => squareDists[square.X, square.Y] < float.PositiveInfinity)
                                                                            .OrderBy((square) => (squareDists[square.X, square.Y]));

            if (sortedPossibleSquares.Count() <= 0)
            {
                //Debug.LogWarning("Warning: No valid squares for object.");
                break;
            }

            int maxIdx = sortedPossibleSquares.Count() - 1;
            float plantDistanceRatio = Config.plantDistanceDifficultyRatioMin + 
                                        ((float)Random.NextDouble() * (Config.plantDistanceDifficultyRatioMax-Config.plantDistanceDifficultyRatioMin));

            int selectedIdx = Math.Max(0, 
                                        Math.Min((int)Math.Round(((maxIdx) * plantDistanceRatio)), 
                                                    maxIdx));

            GridSquare selectedSquare = sortedPossibleSquares.ElementAt(selectedIdx);

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

    public float[,] CalculateDistances(CarGrid grid, GridSquare first)
    {
        return CarGridDijkstras.CalculateDistance(grid, first, (square, intoSquare) => square.ContainedObject.BlocksIrrigation() || intoSquare.ContainedObject.BlocksIrrigation());
    }

    protected abstract int GetNumObjectsToGenerate();

    protected abstract ICarObject GenerateCarObject();
}