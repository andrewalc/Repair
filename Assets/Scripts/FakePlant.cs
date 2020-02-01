using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FakePlant : MonoBehaviour
{
    void Start()
    {
        Tick.Instance.AddEventListener(addWater);
    }

    void addWater()
    {
        WaterResource.Increment();
        print(WaterResource.value);
    }
}
