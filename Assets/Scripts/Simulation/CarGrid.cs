using System;
using System.Collections.Generic;
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

    public float airQuality = 0;
    public float plantMatter = 0;
    public float waterLevel = 0;

    public float Sustainability = 0;

    public int Width => Squares.GetLength(0);
    public int Height => Squares.GetLength(1);

    public CarGrid(int width, int height) {
        Init(width, height);
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
    }

    public bool IsWatered(int x, int y) {
        var seenCells = new HashSet<Tuple<int, int>>();

        if (!Sprinklers[x, y]) {
            return false;
        }
        
        return IsWateredImpl(x, y, seenCells);
    }

    bool IsWateredImpl(int x, int y, HashSet<Tuple<int, int>> seenCells) {
        if (x < 0 || x >= Width || y < 0 || y >= Height || seenCells.Contains(new Tuple<int, int>(x, y))) {
            return false;
        }
        
        if (Squares[x, y].ContainedObject.Type == CarObjectType.Spigot) {
            return true;
        }

        if (IsWatered(x - 1, y) && (PipeConnections[x - 1, y] & PipeConnection.Left) != 0) {
            return true;
        }

        if (IsWatered(x + 1, y) && (PipeConnections[x + 1, y] & PipeConnection.Right) != 0) {
            return true;
        }

        if (IsWatered(x, y + 1) && (PipeConnections[x, y + 1] & PipeConnection.Top) != 0) {
            return true;
        }

        if (IsWatered(x, y - 1) && (PipeConnections[x, y - 1] & PipeConnection.Bottom) != 0) {
            return true;
        }

        return false;
    }

    public void AddPipeBetween(int firstX, int firstY, int secondX, int secondY) {
        bool diffX = firstX != secondX;
        bool diffY = firstY != secondY;

        if (!diffX && diffY) {
            if (firstY > secondY) {
                PipeConnections[firstX, firstY] |= PipeConnection.Bottom;
                PipeConnections[secondX, secondX] |= PipeConnection.Top;
            } else {
                PipeConnections[firstX, firstY] |= PipeConnection.Top;
                PipeConnections[secondX, secondX] |= PipeConnection.Bottom;
            }
        } else if (!diffY && diffX) {
            if (firstX > secondX) {
                PipeConnections[firstX, firstY] |= PipeConnection.Left;
                PipeConnections[secondX, secondX] |= PipeConnection.Right;
            } else {
                PipeConnections[firstX, firstY] |= PipeConnection.Right;
                PipeConnections[secondX, secondX] |= PipeConnection.Left;
            }
        }
    }

    public void UpgradeMachineAt(int x, int y) {
        var containedObject = Squares[x, y].ContainedObject;
        if (containedObject.Type == CarObjectType.Machine) {
            var machine = (MachineCarObject) containedObject;
            machine.level++;
            machine.level = Mathf.Clamp(machine.level, 1, 5);
        }
    }

    public CarGrid Clone() {
        var clone = new CarGrid(Squares.GetLength(0), Squares.GetLength(1));

        for (int x = 0; x < Width; ++x) {
            for (int y = 0; y < Height; ++y) {
                clone.Squares[x,y] = Squares[x, y].Clone();
            }
        }
        
        // TODO clone irrigation stuff
        
        clone.plantMatter = plantMatter;
        clone.waterLevel = waterLevel;
        
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
}
