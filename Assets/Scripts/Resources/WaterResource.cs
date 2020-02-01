using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Water", menuName = "Resources/Water", order = 1)]
public class WaterResource : ScriptableObject
{
    public float value;
    public float rate;
    public float max;

    public void Tick()
    {
        value += rate;
        
        if (value < 0)
        {
            value = 0;
        }

        if (value > max)
        {
            value = max;
        }
    }
}
