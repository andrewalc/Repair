using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarElementsGenerator : MonoBehaviour
{
    public List<GameObject> plant_prefabs;
    public List<GameObject> obstacle_prefabs;
    public GameObject machine_prefab;
    private bool generated;
    
    // Start is called before the first frame update
    void Start()
    {
        generated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (generated || !Game.Instance.finishedGeneratingLevel) return;
        generateGrid();
        generated = true;
    }

    void generateGrid()
    {
        CarGrid grid = Game.Instance.Simulation.currentState;
        
        for (int x = 0; x < grid.Squares.GetLength(0); ++x) {
            for (int y = grid.Squares.GetLength(1) - 1; y >= 0; --y) 
            {
                GridSquare square = grid.Squares[x, y].Clone();
                generateCarElement(square);
            }
        }
    }

    void generateCarElement(GridSquare square)
    {
        CarObjectType type = square.ContainedObject.Type;
        CarGrid grid = Game.Instance.Simulation.currentState;
        var position = transform.position;
        Vector3 objectPosition = new Vector3(
            position.x + (square.X * 2.4f),
            position.y + 1,
            (position.z + (square.Y - (grid.Squares.GetLength(1)))* 2.2f)
        );
        GameObject prefab;
        GameObject child;
        System.Random r = new System.Random();
        int index;
        
        switch (type)
        {
            case CarObjectType.Plant:
                index = r.Next(0, plant_prefabs.Count);
                prefab = plant_prefabs[index];
                child = Instantiate(prefab, objectPosition, Quaternion.identity);
                child.GetComponent<PlantState>().square = square;
                break;
            case CarObjectType.Obstacle:
                index = r.Next(0, obstacle_prefabs.Count);
                prefab = obstacle_prefabs[index];
                child = Instantiate(prefab, objectPosition, Quaternion.identity);

                break;
            case CarObjectType.Machine:
                print(((MachineCarObject) square.ContainedObject).MachineType);
                prefab = machine_prefab;
                child = Instantiate(prefab, objectPosition, Quaternion.identity);
                child.GetComponent<MachineObject>().square = square;

                break;
            default:
                prefab = null;
                child = null;
                break;
        }
        if (child == null) return;
//        float scale = .05f;
//        child.transform.localScale = new Vector3(scale, scale, scale);
        child.transform.parent = transform;
    }
}
