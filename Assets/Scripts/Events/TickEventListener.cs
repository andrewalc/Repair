using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TickEventListener : MonoBehaviour
{
    public TickEvent TickEvent;
    public UnityEvent Response;

    private void OnEnable()
    {
        TickEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        TickEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
