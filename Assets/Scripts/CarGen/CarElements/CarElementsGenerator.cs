using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarElementsGenerator : MonoBehaviour
{
    public List<GameObject> plant_prefabs;
    public List<GameObject> obstacle_prefabs;
    public GameObject aeromachine_prefab;
    public GameObject hydromachine_prefab;
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
        GameObject prefab;

        System.Random r = new System.Random();
        int index;
        
        switch (type)
        {
            case CarObjectType.Plant:
                index = r.Next(0, plant_prefabs.Count);
                prefab = plant_prefabs[index];
                break;
            case CarObjectType.Obstacle:
                index = r.Next(0, obstacle_prefabs.Count);
                prefab = obstacle_prefabs[index];
                break;
            case CarObjectType.Machine:
                prefab = aeromachine_prefab;
                break;
            default:
                prefab = null;
                break;
        }

        CarGrid grid = Game.Instance.Simulation.currentState;
        var position = transform.position;
        Vector3 objectPosition = new Vector3(
            position.x + (square.X * 2),
            position.y + 1,
            (position.z + (square.Y - (grid.Squares.GetLength(1)))* 2)
        );

        if (prefab == null) return;
        GameObject child = Instantiate(prefab, objectPosition, Quaternion.identity);
        child.transform.parent = transform;
    }
}
