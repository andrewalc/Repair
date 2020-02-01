using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TickSpeed", menuName = "TickSpeed", order = 1)]
public class TickEvent : ScriptableObject
{
    private List<TickEventListener> listeners = new List<TickEventListener>();

    public void Raise()
    {
        for (int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(TickEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(TickEventListener listener)
    {
        listeners.Remove(listener);
    }
}