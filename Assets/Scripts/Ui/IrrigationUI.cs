using UnityEngine;
using UnityEngine.UI;

public class IrrigationUI : MonoBehaviour {
    [SerializeField] Text pipeCost;
    [SerializeField] Text sprinklerCost;

    [SerializeField] GridLayoutGroup cellGrid;
    [SerializeField] GameObject CellPrefab;

    public void Init() {
        foreach (Transform child in cellGrid.transform) {
            Destroy(child.gameObject);
        }
        
        CarGrid grid = Game.Instance.Simulation.currentState;
        for (int y = 0; y < grid.Height; ++y) {
            for (int x = 0; x < grid.Width; ++x) {
                var button = Instantiate(CellPrefab, cellGrid.transform);
                var cell = button.GetComponent<IrrigationCellUi>();
                cell.Init(grid.Squares[x, y].ContainedObject.Type);
            }
        }
    }
}
