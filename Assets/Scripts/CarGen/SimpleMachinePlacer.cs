using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SimpleMachinePlacer : CoroutineCarGenerator
{
    private System.Random random;

    public SimpleMachinePlacer(MonoBehaviour host, CarGeneratorConfig config, CarGrid gridToUse, System.Random random) : base(host, config, gridToUse)
    {
        this.random = random;
    }

    protected override IEnumerator PlaceObjects()
    {
        int numMachinesToGenerate = random.Next(Config.numMachinesMin, Config.numMachinesMax);

        for ( int i = 0; i < numMachinesToGenerate; ++i)
        {
            IEnumerable<GridSquare> possibleSquares = ResultGrid.SquaresEnumerable().Where(
                                                            (square) => null == square.ContainedObject || square.ContainedObject.IsEmpty()
                                                        );

            int selectedSquareIdx = random.Next(possibleSquares.Count() - 1);
            GridSquare selectedSquare = possibleSquares.ElementAt(selectedSquareIdx);
            Debug.Log("Placing machine at square: " + selectedSquare.X + ", " + selectedSquare.Y);

            MachineCarObject machine = GenerateMachine();
            
            selectedSquare.ContainedObject = machine;

            ResultGrid.SetSquare(selectedSquare);

            yield return null;
        }
    }

    private MachineCarObject GenerateMachine()
    {
        // TODO: Give the machine properties if necessary.
        return new MachineCarObject();
    }
}