﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarElementsGenerator : MonoBehaviour
{
    public List<GameObject> plant_prefabs;
    public List<GameObject> obstacle_prefabs;
    public GameObject machine_prefab;
    private bool awaitingRequestGeneration;
    private CarGrid carGrid;
    
    // Start is called before the first frame update
    void Start()
    {
        awaitingRequestGeneration = false;
        if (Game.Instance.Simulation != null)
        {
            Game.Instance.Simulation.PlantSpawnEvent += generateCarElement;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InstantiateLevel()
    {
        Game.Instance.LevelGenerated += OnLevelGenerated;
        if (Game.Instance.Simulation != null)
        {
            Game.Instance.Simulation.PlantSpawnEvent -= generateCarElement;
        }

        Game.Instance.GenerateNewCar();
        awaitingRequestGeneration = true;

    }

    public void OnLevelGenerated(Simulation newSim)
    {
        if (awaitingRequestGeneration)
        {
            carGrid = newSim.currentState;
            generateGrid();
            awaitingRequestGeneration = false;
            newSim.PlantSpawnEvent += generateCarElement;
        }
    }
    
    void generateGrid()
    {
        for (int x = 0; x < carGrid.Squares.GetLength(0); ++x) {
            for (int y = carGrid.Squares.GetLength(1) - 1; y >= 0; --y) 
            {
                generateCarElement(carGrid.Squares[x, y]);
            }
        }
    }

    void generateCarElement(GridSquare square)
    {
        CarObjectType type = square.ContainedObject.Type;
        var position = transform.position;
        Vector3 objectPosition = new Vector3(
            position.x + (square.X * 2.4f),
            position.y + 1,
            (position.z + (square.Y - (carGrid.Squares.GetLength(1)))* 2.2f)
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
