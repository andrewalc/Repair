using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrrigationCellUiGen : MonoBehaviour
{
    public GameObject CellPrefab;
    public bool updated;
    
    // Start is called before the first frame update
    void Start()
    {
        updated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!updated && Game.Instance.finishedGeneratingLevel)
        {
            UpdateCellGrid();
            updated = true;
        }

    }

    public void UpdateCellGrid()
    {
        CarGrid grid = Game.Instance.simulation.currentState;
        for (int x = 0; x < grid.Squares.GetLength(0); ++x) {
            for (int y = grid.Squares.GetLength(1) - 1; y >= 0; --y) 
            {
                GameObject button = Instantiate(CellPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                IrrigationCellUi cell = button.GetComponent<IrrigationCellUi>();
                cell.square = grid.Squares[x, y].Clone();
                button.transform.parent = transform;
            }
        }
    }
}
