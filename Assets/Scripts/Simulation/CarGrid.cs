using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DarkConfig;
using UnityEngine;

public enum IrrigationCell {
    Empty,
    Pipe,
    Sprinkler
}

[Flags]
public enum PipeConnection {
    None = 0,
    Top = 1 << 0,
    Right = 1 << 1,
    Bottom = 1 << 2,
    Left = 1 << 3
}

public class CarGrid {
    public GridSquare[,] Squares;

    public bool[,] Sprinklers;
    public PipeConnection[,] PipeConnections;

    private float[,] minWaterDists;
    
    public float Sustainability = 0;

    private ResourceManager resources = new ResourceManager();

    public int Width => Squares.GetLength(0);
    public int Height => Squares.GetLength(1);

    public event Simulation.ResourceChangedEvent ResourceChanged;

    public CarGrid(int width, int height) {
        Init(width, height);
        
        resources.ResourceChanged += OnResourceChanged;
    }

    public void Clear()
    {
        Init(Width, Height);
    }

    private void Init(int width, int height)
    {
        Squares = new GridSquare[width, height];
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                Squares[x, y] = new GridSquare(x, y);
            }
        }

        Sprinklers = new bool[width, height];
        PipeConnections = new PipeConnection[width, height];

        InitWaterDists();
    }

    private delegate bool ObjectTypePredicate(GridSquare square);

    private void CalculateMinDistsFromWater()
    {
        float[,] waterSourceDists = CalculateMinDistsFromObjects(square => square.ContainedObject.IsWaterSource(),
                                                                    (square, intoSquare) => square.ContainedObject.BlocksIrrigation() || intoSquare.ContainedObject.BlocksIrrigation());
        for (int x = 0; x < Width; ++x)
        {
            for (int y = 0; y < Height; ++y)
            {
                Squares[x, y].MinDistFromWaterSource = waterSourceDists[x, y];
            }
        }
    }
    
    private void CalculateMinDistsFromPlants()
    {
        float[,] plantDists = CalculateMinDistsFromObjects(square => square.ContainedObject.Type == CarObjectType.Plant,
                                                            (square, intoSquare) => (square.ContainedObject.Type != CarObjectType.Plant && square.ContainedObject.Type != CarObjectType.Empty) ||
                                                                                                (intoSquare.ContainedObject.Type != CarObjectType.Plant && intoSquare.ContainedObject.Type != CarObjectType.Empty));
        for (int x = 0; x < Width; ++x)
        {
            for (int y = 0; y < Height; ++y)
            {
                Squares[x, y].MinDistFromInitialPlants = plantDists[x, y];
            }
        }
    }

    private float[,] CalculateMinDistsFromObjects(ObjectTypePredicate typePredicate, CarGridDijkstras.PathBlockingPredicate blocksPath)
    {
        IEnumerable<GridSquare> relevantObjects =
            this.SquaresEnumerable().Where((square) => typePredicate(square));
        float[,,] relevantObjectDists = new float[Width,Height,relevantObjects.Count()];
        int i = 0;
        foreach (GridSquare relevantObject in relevantObjects)
        {
            float[,] objectDists = CarGridDijkstras.CalculateDistance(this, relevantObject, blocksPath);
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    relevantObjectDists[x, y, i] = objectDists[x, y];
                }
            }

            ++i;
        }

        float[,] results = new float[Width,Height];
        for (int x = 0; x < Width; ++x)
        {
            for (int y = 0; y < Height; ++y)
            {
                results[x,y] = float.PositiveInfinity;
                for (int sourceIdx = 0; sourceIdx < relevantObjectDists.GetLength(2); ++sourceIdx)
                {
                    results[x,y] = Math.Min(results[x,y], relevantObjectDists[x, y, sourceIdx]);
                }
            }
        }

        return results;
    }

    public bool IsWatered(int x, int y)
    {
        if (!Sprinklers[x, y]) {
            return false;
        }
        
        return !float.IsPositiveInfinity(minWaterDists[x, y]);
    }

    public bool IsPipeConnected(GridSquare first, GridSquare second)
    {
        if (first.ContainedObject.BlocksIrrigation() ||
            second.ContainedObject.BlocksIrrigation())
        {
            return false;
        }

        return IsPipePlaced(first, second);
    }

    private bool IsPipePlaced(GridSquare first, GridSquare second)
    {
        if (first.Y == second.Y)
        {
            if (first.X + 1 == second.X)
            {
                return (PipeConnections[first.X, first.Y] & PipeConnection.Right) != 0;
            }
            else if (first.X - 1 == second.X)
            {
                return (PipeConnections[first.X, first.Y] & PipeConnection.Left) != 0;
            }
        }
        else if (first.X == second.X)
        {
            if (first.Y + 1 == second.Y)
            {
                return (PipeConnections[first.X, first.Y] & PipeConnection.Top) != 0;
            }
            else if (first.Y - 1 == second.Y)
            {
                return (PipeConnections[first.X, first.Y] & PipeConnection.Bottom) != 0;
            }
        }

        return false;
    }
    

    private void InitWaterDists()
    {
        minWaterDists = new float[this.Width, this.Height];
        for (int x = 0; x < this.Width; ++x)
        {
            for (int y = 0; y < this.Height; ++y)
            {
                minWaterDists[x, y] = float.PositiveInfinity;
            }
        }
    }
    
    public float[,] CalculateWaterDists()
    {
        InitWaterDists();

        foreach (GridSquare spigotSquare in this.SquaresEnumerable()
            .Where((square) => square.ContainedObject.IsWaterSource()))
        {
            float[,] waterDists = CarGridDijkstras.CalculateDistance(this, spigotSquare, (first, second) =>
            {
                return !IsPipeConnected(first, second);
            });
            for (int x = 0; x < this.Width; ++x)
            {
                for (int y = 0; y < this.Height; ++y)
                {
                    minWaterDists[x, y] = Math.Min(minWaterDists[x, y], waterDists[x, y]);
                }
            }
        }

        return minWaterDists;
    }

    public float GetWaterTravelDist(int x, int y)
    {
        return minWaterDists[x, y];
    }
    
    public bool TogglePipeBetween(int firstX, int firstY, int secondX, int secondY) {
        bool diffX = firstX != secondX;
        bool diffY = firstY != secondY;

        if (IsPipePlaced(Squares[firstX, firstY], Squares[secondX, secondY]))
        {
            ChangeResource(ResourceType.PlantMatter, Game.Instance.SimulationSettings.pipePrice *
                           Game.Instance.SimulationSettings.irrigationRefundMultiplier);
        }
        else
        {
            // can we afford it?
            if (GetResourceValue(ResourceType.PlantMatter) < Game.Instance.SimulationSettings.pipePrice)
            {
                Debug.Log("Can't afford the pipe");
                return false;
            }

            ChangeResource(ResourceType.PlantMatter, -Game.Instance.SimulationSettings.pipePrice);
        }

        if (!diffX && diffY) {
            if (firstY > secondY) {
                // Y goes down as we approach the bottom of the car, so if the first is higher, it now has a bottom connection.
                PipeConnections[firstX, firstY] ^= PipeConnection.Bottom;
                PipeConnections[secondX, secondY] ^= PipeConnection.Top;
            } else {
                PipeConnections[firstX, firstY] ^= PipeConnection.Top;
                PipeConnections[secondX, secondY] ^= PipeConnection.Bottom;
            }
        } else if (!diffY && diffX) {
            if (firstX > secondX) {
                PipeConnections[firstX, firstY] ^= PipeConnection.Left;
                PipeConnections[secondX, secondY] ^= PipeConnection.Right;
            } else {
                PipeConnections[firstX, firstY] ^= PipeConnection.Right;
                PipeConnections[secondX, secondY] ^= PipeConnection.Left;
            }
        }

        CalculateWaterDists();

        return true;
    }

    public bool ToggleSprinkler(int x, int y)
    {
        if (Sprinklers[x, y])
        {
            ChangeResource(ResourceType.PlantMatter, Game.Instance.SimulationSettings.sprinklerPrice * Game.Instance.SimulationSettings.irrigationRefundMultiplier);
        }
        else
        {
            if (GetResourceValue(ResourceType.PlantMatter) < Game.Instance.SimulationSettings.sprinklerPrice)
            {
                Debug.Log("Can't afford the sprinkler");
                return false;
            }
            
            ChangeResource(ResourceType.PlantMatter, -Game.Instance.SimulationSettings.sprinklerPrice);
        }

        Sprinklers[x, y] = !Sprinklers[x,y];
        return true;
    }
    
    public void UpgradeMachineAt(int x, int y) {
        var containedObject = Squares[x, y].ContainedObject;
        if (containedObject.Type == CarObjectType.Machine) {
            var machine = (MachineCarObject) containedObject;
            machine.level++;
            machine.level = Mathf.Clamp(machine.level, 1, Game.Instance.Simulation.config.maxMachineLevel);
        }
    }

    public CarGrid Clone() {
        var clone = new CarGrid(Squares.GetLength(0), Squares.GetLength(1));

        for (int x = 0; x < Width; ++x) {
            for (int y = 0; y < Height; ++y) {
                clone.Squares[x,y] = Squares[x, y].Clone();

                clone.PipeConnections[x, y] = PipeConnections[x, y];
                clone.Sprinklers[x, y] = Sprinklers[x, y];

                clone.minWaterDists[x, y] = this.minWaterDists[x, y];
            }
        }
        
        clone.resources.CopyFrom(this.resources);
        
        return clone;
    }

    public override string ToString() {
        var sb = new StringBuilder();
        for (int y = Height - 1; y >= 0; --y) {
            for (int x = 0; x < Width; ++x) {
                switch (Squares[x, y].ContainedObject.Type) {
                    case CarObjectType.Empty: sb.Append("."); break;
                    case CarObjectType.Obstacle: sb.Append("#"); break;
                    case CarObjectType.Plant: sb.Append("p"); break;
                    case CarObjectType.Spigot: sb.Append("s"); break;
                    case CarObjectType.Machine: sb.Append("m"); break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    public static CarGrid FromDoc(CarGrid existing, DocNode doc) {
        var docString = doc.AsString();
        int width = docString.IndexOf('\n');

        var lines = docString.Split('\n');
        int height = lines.Length;
        
        existing = new CarGrid(width, height);
        
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                ICarObject carObject;
                
                // lines is row=major, but we want column-major for the grid.
                var c = lines[height - y - 1][x];
                switch (c) {
                    case '.': carObject = new EmptyCarObject(); break;
                    case 'p': carObject = new PlantCarObject(); break;
                    case 'm': carObject = new MachineCarObject(); break;
                    case '#': carObject = new ObstacleCarObject(); break;
                    case 's': carObject = new SpigotCarObject(); break;
                    default: throw new ArgumentOutOfRangeException();
                }
                existing.Squares[x, y] = new GridSquare(x, y) {
                    ContainedObject = carObject
                };
            }
        }
        return existing;
    }

    public void RecalculateExtraInfo()
    {
        CalculateMinDistsFromPlants();
        CalculateMinDistsFromWater();
    }

    public ResourceEntry ChangeResource(ResourceType type, float value)
    {
        return resources.AddToResource(type, value);
    }
    
    public ResourceEntry SetResource(ResourceType type, float value)
    {
        return resources.SetResource(type, value);
    }

    public float GetResourceValue(ResourceType type)
    {
        return resources.GetValue(type);
    }

    public void OnResourceChanged(float oldValue, ResourceEntry resource)
    {
        ResourceChanged?.Invoke(oldValue, resource);
    }

    public int CalculatePossiblePlantPlots()
    {
        return this.SquaresEnumerable().Where((square) =>
                     !square.ContainedObject.BlocksIrrigation() && !square.ContainedObject.IsWaterSource())
                 .Count((square) => square.MinDistFromWaterSource < float.PositiveInfinity &&
                                    square.MinDistFromInitialPlants < float.PositiveInfinity);

    }
}
