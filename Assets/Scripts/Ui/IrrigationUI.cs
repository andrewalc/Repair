using UnityEngine;
using UnityEngine.UI;

public class IrrigationUI : MonoBehaviour
{
    [SerializeField] Text pipeCost;
    [SerializeField] Text sprinklerCost;

    [SerializeField] GridLayoutGroup cellGrid;
    [SerializeField] GridLayoutGroup pipeGrid;
    [SerializeField] GameObject CellPrefab;
    [SerializeField] GameObject PipePrefab;

    private IrrigationCellUi[,] irrigationCells;

    void Awake()
    {
        HoverManager.Instance.disabled = true;
    }

    void OnDestroy()
    {
        Game.Instance.Simulation.plantSpawnEvent -= OnPlantSpawned;
        HoverManager.Instance.disabled = false;
    }

    public void Close()
    {
        Game.Instance.Simulation.plantSpawnEvent -= OnPlantSpawned;
        UiDisable.Instance.disabled = false;
        Destroy(gameObject);
    }

    public void Init()
    {
        Game.Instance.Simulation.plantSpawnEvent += OnPlantSpawned;
        
        foreach (Transform child in cellGrid.transform)
        {
            Destroy(child.gameObject);
        }

        CarGrid grid = Game.Instance.Simulation.currentState;
        
        irrigationCells = new IrrigationCellUi[grid.Width, grid.Height];
        
        for (int y = 0; y < grid.Height; ++y)
        {
            for (int x = 0; x < grid.Width; ++x)
            {
                var button = Instantiate(CellPrefab, cellGrid.transform);
                var cell = button.GetComponent<IrrigationCellUi>();
                cell.Init(grid.Squares[x, y].ContainedObject.Type);
                irrigationCells[x, y] = cell;
            }
        }

        for (int y = 0; y < grid.Height * 2 - 1; ++y)
        {
            for (int x = 0; x < grid.Width * 2 - 1; ++x)
            {
                var button = Instantiate(PipePrefab, pipeGrid.transform);

                PipeState state = CalculatePipeState(x, y, grid);

                button.GetComponent<PipeButton>().Init(CalculatePipeState(x, y, grid), x, y);
            }
        }
    }

    private void OnPlantSpawned(GridSquare square)
    {
        irrigationCells[square.X, square.Y].Init(square.ContainedObject.Type);
    }

    private PipeState CalculatePipeState(int x, int y, CarGrid grid)
    {
        if (x % 2 == 0 && y % 2 == 0)
        {
            return grid.Sprinklers[x / 2, y / 2] ? PipeState.Sprinkler : PipeState.Empty;
        }
        else
        {
            if (x % 2 == 0)
            {
                return (grid.PipeConnections[x / 2, y / 2] & PipeConnection.Top) != 0
                    ? PipeState.Pipe
                    : PipeState.Empty;
            }
            else
            {
                return (grid.PipeConnections[x / 2, y / 2] & PipeConnection.Right) != 0
                    ? PipeState.Pipe
                    : PipeState.Empty;
            }
        }
    }
}