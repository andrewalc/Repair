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
                    case 's': carObject = new ObstacleCarObject(); break;
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
