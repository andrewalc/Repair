using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiDisable : MonoBehaviour
{
    public bool disabled;
    public static UiDisable Instance { get; private set; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Can only have one game instance!");
        }
    }
}
