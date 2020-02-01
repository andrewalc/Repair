using System;
using DarkConfig;

public class CarGrid {
    public GridSquare[,] Squares;

    public CarGrid(int width, int height) {
        Squares = new GridSquare[width, height];
        for (int x = 0; x < width; ++x) {
            for (int y = 0; y < height; ++y) {
                Squares[x, y] = new GridSquare(x, y);
            }
        }
    }

    public CarGrid Clone() {
        var clone = new CarGrid(Squares.GetLength(0), Squares.GetLength(1));

        for (int x = 0; x < Squares.GetLength(0); ++x) {
            for (int y = 0; y < Squares.GetLength(1); ++y) {
                clone.Squares[x,y] = Squares[x, y].Clone();
            }
        }
        
        return clone;
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
                    case 'o': carObject = new ObstacleCarObject(); break;
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
