using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HydroMachine : MonoBehaviour
{
    private float PollutionRate = -1.5f;
    void Start()
    {
        Tick.Instance.AddEventListener(addPollution);
    }

    void addPollution()
    {
        AirQualityResource.changeAirQuality(PollutionRate);
        print(AirQualityResource.value);
    }
}