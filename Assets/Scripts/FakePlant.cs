using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FakePlant : MonoBehaviour
{
    private float baseWaterGen = -1;
    void Start()
    {
        Tick.Instance.AddEventListener(AddWater);
    }

    void AddWater()
    {
        WaterResource.updateWater(baseWaterGen);
    }
}
