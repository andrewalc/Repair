using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGenerator : MonoBehaviour
{
    public GameObject trainPrefab;
    public GameObject startingTrain;
    private List<GameObject> trains;
    public int currentTrainIndex;
    // Start is called before the first frame update
    void Start()
    {
        if (startingTrain == null)
        {
            Debug.LogError("Starting Train Required");
        }
        trains = new List<GameObject>();
        trains.Add(startingTrain);
        CreateTrainCar(); // Have one in advance
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateTrainCar();
        }
    }

    public void CreateTrainCar()
    {
        Vector3 trainPos = startingTrain.transform.position;
        print(trains);
        GameObject newTrain = Instantiate(trainPrefab, new Vector3(trainPos.x - (33f * trains.Count), trainPos.y, trainPos.z), Quaternion.identity);
        newTrain.transform.parent = transform;
        trains.Add(newTrain);
    }

    public void GenerateLevel(int index)
    {
        trains[index].GetComponentInChildren<CarElementsGenerator>().InstantiateLevel();
    }
}
