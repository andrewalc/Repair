using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;
using UnityEngine;

public static class CarGridDijkstras
{
    private class PriorityQueue : IEnumerable<Tuple<int, int>>
    {
        private List<Tuple<int,int>> items;

        private float[,] dists;

        public PriorityQueue(IEnumerable<Tuple<int,int>> positions, float[,] dists)
        {
            items = positions.ToList();
            this.dists = dists;
            Heapify();
        }

        public void Heapify()
        {
            items = items.OrderBy((position) => dists[position.Item1, position.Item2]).ToList();
            //items.Sort((left, right) => { return -1*(dists[left.Item1, left.Item2].CompareTo(dists[right.Item1, right.Item2])); } );
        }

        public Tuple<int, int> Pop()
        {
            Tuple<int,int> position = items.First();
            items.RemoveAt(0);

            return position;
        }

        public void Push(Tuple<int,int> position)
        {
            items.Add(position);
        }

        public void UpdateDist(Tuple<int, int> position, float dist)
        {
            this.dists[position.Item1, position.Item2] = dist;
        }

        public IEnumerator<Tuple<int,int>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public static float[,] CalculateDistance(CarGrid grid, GridSquare first)
    {
        float[,] dists = new float[grid.Width(), grid.Height()];
        HashSet<Tuple<int, int>> closedList = new HashSet<Tuple<int,int>>();

        for( int x = 0; x < grid.Width(); x++)
        {
            for (int y = 0; y < grid.Height(); y++ )
            {
                dists[x,y] = float.PositiveInfinity;
            }
        }

        dists[first.X, first.Y] = 0;

        PriorityQueue squareQueue = new PriorityQueue( grid.SquarePositions(), dists );
        while (squareQueue.Count() > 0)
        {
            Tuple<int, int> itemPos = squareQueue.Pop();
            closedList.Add(itemPos);
            float currDist = dists[itemPos.Item1, itemPos.Item2];

            GridSquare square = grid.Squares[itemPos.Item1, itemPos.Item2];
            foreach (GridSquare neighbor in grid.GetNeighbors(square))
            {
                if (closedList.Contains(neighbor.GetPos()))
                {
                    continue;
                }

                float newNeighborDist = currDist + ComputeStraightLineDist(grid, square.GetPos(), neighbor.GetPos());
                if (newNeighborDist < dists[neighbor.X, neighbor.Y])
                {
                    dists[neighbor.X, neighbor.Y] = newNeighborDist;
                    squareQueue.UpdateDist(neighbor.GetPos(), dists[neighbor.X, neighbor.Y]);
                }
                squareQueue.Heapify();
            }
        }

        return dists;
    }

    private static float ComputeStraightLineDist(CarGrid grid, Tuple<int, int> first, Tuple<int,int> second)
    {
        if (grid.Squares[first.Item1, first.Item2].ContainedObject.BlocksIrrigation())
        {
            return float.PositiveInfinity;
        }

        if (grid.Squares[second.Item1, second.Item2].ContainedObject.BlocksIrrigation())
        {
            return float.PositiveInfinity;
        }

        return Math.Abs(first.Item1 - second.Item1) + Math.Abs(first.Item2 - second.Item2);
    }
}