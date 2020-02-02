using UnityEngine;
using UnityEngine.UI;

public class IrrigationUI : MonoBehaviour {
    [SerializeField] Text pipeCost;
    [SerializeField] Text sprinklerCost;

    [SerializeField] GridLayoutGroup cellGrid;
    [SerializeField] GridLayoutGroup pipeGrid;
    [SerializeField] GameObject CellPrefab;
    [SerializeField] GameObject PipePrefab;

    void Awake() {
        HoverManager.Instance.disabled = true;
    }

    void OnDestroy() {
        HoverManager.Instance.disabled = false;
    }

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

        for (int y = 0; y < grid.Height * 2 - 1; ++y) {
            for (int x = 0; x < grid.Width * 2 - 1; ++x) {
                var button = Instantiate(PipePrefab, pipeGrid.transform);

                button.GetComponent<PipeButton>().Init(pipeState.Empty, x, y);
                
                bool visible = (y % 2 == 0) != (x % 2 == 0);
                
                if (!visible) {
                    button.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
                }
            }
        }
    }
}
