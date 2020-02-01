using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TickSpeed", menuName = "TickSpeed", order = 1)]
public class TickSpeed : ScriptableObject
{
    public float timePerTick;
    public TickEvent tickEvent;
    private float _timeUntilTick;

    public void UpdateTick(float timePassed)
    {
        _timeUntilTick -= timePassed;

        if (_timeUntilTick < 0)
        {
            _timeUntilTick += timePerTick;
            tickEvent.Raise();
        }
    }
}