using System;
using System.Text;
using DarkConfig;

public class CarGrid {
    public GridSquare[,] Squares;

    public bool[,] Watered;

    public float airQuality = 0;
    public float plantMatter = 0;
    public float waterLevel = 0;

    public int Width => Squares.GetLength(0);
    public int Height => Squares.GetLength(1);

    public CarGrid(int width, int height) {
        Squares = new GridSquare[width, height];
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                Squares[x, y] = new GridSquare(x, y);
            }
        }

        Watered = new bool[width, height];
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                Watered[x, y] = false;
            }
        }
    }

    public CarGrid Clone() {
        var clone = new CarGrid(Squares.GetLength(0), Squares.GetLength(1));

        for (int x = 0; x < Width; ++x) {
            for (int y = 0; y < Height; ++y) {
                clone.Squares[x,y] = Squares[x, y].Clone();
            }
        }

        for (int x = 0; x < Width; ++x) {
            for (int y = 0; y < Height; ++y) {
                clone.Watered[x,y] = Watered[x, y];
            }
        }
        
        clone.plantMatter = plantMatter;
        clone.waterLevel = waterLevel;
        
        return clone;
    }

    public override string ToString() {
        var sb = new StringBuilder();
        for (int x = 0; x < Squares.GetLength(0); ++x) {
            for (int y = Squares.GetLength(1) - 1; y >= 0; --y) {
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
