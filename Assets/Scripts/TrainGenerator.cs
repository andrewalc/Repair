using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGenerator : MonoBehaviour
{
    public GameObject trainPrefab;
    public GameObject startingTrain;
    private List<GameObject> trains;
    
    // Start is called before the first frame update
    void Start()
    {
        if (startingTrain == null)
        {
            Debug.LogError("Starting Train Required");
        }
        trains = new List<GameObject>();
        trains.Add(startingTrain);
        

        Game.Instance.BeginPlay += OnBeginPlay;
    }

    public void OnBeginPlay()
    {
        if (trains.Count == 1)
        {
            // Create a second train car and generate the contents in the first.
            CreateTrainCar();
        }
    }

    public void CreateTrainCar()
    {
        Vector3 trainPos = startingTrain.transform.position;
        
        print("New train: " + trains.Count);
        
        GameObject newTrain = Instantiate(trainPrefab, new Vector3(trainPos.x - (33f * trains.Count), trainPos.y, trainPos.z), Quaternion.identity);
        newTrain.transform.parent = transform;
        trains.Add(newTrain);
        
        // Always generate for the second-newest train, because we want an empty car at the front.
        GenerateLevel(trains.Count - 2);
    }

    public void GenerateLevel(int index)
    {
        trains[index].GetComponentInChildren<CarElementsGenerator>().InstantiateLevel();
    }

}
