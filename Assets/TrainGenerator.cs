using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGenerator : MonoBehaviour
{
    public GameObject trainPrefab;
    public GameObject startingTrain;
    private List<GameObject> trains;
    public int currentTrainIndex;

    public bool generated;
    public bool firstCar;
    // Start is called before the first frame update
    void Start()
    {
        if (startingTrain == null)
        {
            Debug.LogError("Starting Train Required");
        }
        trains = new List<GameObject>();
        trains.Add(startingTrain);
        StartCoroutine(GenFirstCar());
        
    }


    IEnumerator GenFirstCar()
    {
        yield return null;
        GenerateLevel(0);
        yield return new WaitUntil( () => Game.Instance.finishedGeneratingLevel);
        CreateTrainCar();
        while (true)
        {
            yield return null;
            if (!generated && Game.Instance.finishedGeneratingLevel)
            {
            GenerateLevel(trains.Count - 1);
            generated = true;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            CreateTrainCar();
//            FindObjectOfType<CameraShift>().AnimateForward();
//        }
//
//        if (Input.GetMouseButtonDown(1))
//        {
//            FindObjectOfType<CameraShift>().AnimateBackward();
//        }
    }

    public void CreateTrainCar()
    {
        generated = false;
        Vector3 trainPos = startingTrain.transform.position;
        print(trains.Count);    
        GameObject newTrain = Instantiate(trainPrefab, new Vector3(trainPos.x - (33f * trains.Count), trainPos.y, trainPos.z), Quaternion.identity);
        newTrain.transform.parent = transform;
        trains.Add(newTrain);
    }

    public void GenerateLevel(int index)
    {
        trains[index].GetComponentInChildren<CarElementsGenerator>().InstantiateLevel();
    }
}
