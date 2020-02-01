using System;

public class Simulation {
    public CarGrid currentState;
    public SimulationSettingsConfig config;

    public Simulation(CarGrid grid, SimulationSettingsConfig config) {
        currentState = grid.Clone();
        this.config = config;
    }

    public void Step() {
        var newState = currentState.Clone();

        for (int x = 0; x < currentState.Squares.GetLength(0); ++x) {
            for (int y = currentState.Squares.GetLength(1) - 1; y >= 0; --y) {
                
                bool spawnPlant = currentState.Squares[x, y].ContainedObject.Type == CarObjectType.Empty &&
                                  NumSurroundingObjects(x, y, CarObjectType.Plant) > 0;

                if (spawnPlant) {
                    newState.Squares[x, y].ContainedObject = new PlantCarObject();
                }
                
                
            }
        }

        currentState = newState;
    }

    int NumSurroundingObjects(int x, int y, CarObjectType type) {
        int totalSurrounding = 0;

        if (x > 0 && currentState.Squares[x - 1, y].ContainedObject.Type == type) {
            totalSurrounding++;
        }

        if (x < currentState.Squares.GetLength(0) - 1 && currentState.Squares[x + 1, y].ContainedObject.Type == type) {
            totalSurrounding++;
        }

        if (y > 0 && currentState.Squares[x, y - 1].ContainedObject.Type == type) {
            totalSurrounding++;
        }

        if (y < currentState.Squares.GetLength(1) - 1 && currentState.Squares[x, y + 1].ContainedObject.Type == type) {
            totalSurrounding++;
        }

        return totalSurrounding;
    }
}

