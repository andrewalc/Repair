﻿using System;
using System.Collections;
using UnityEngine;

public class Tick : MonoBehaviour
{
    private float _timePerTick = 1.0f;
    private float _timeUntilTick;
    private event Action TickEvent;
    private bool paused;
    
    public static Tick Instance { get; private set; }

    private void Start()
    {
        _timeUntilTick = _timePerTick;
        paused = true;
    }

    public void UnPause()
    {
        paused = false;
    }
    public void FixedUpdate()
    {
        if (paused)
        {
            return;
        }
        
        _timeUntilTick -= Time.deltaTime;

        if (_timeUntilTick < 0)
        {
            _timeUntilTick += _timePerTick;
            if (TickEvent != null)
            {
                print("ticking source");
                TickEvent();
            }
        }
    }

    public void AddEventListener(Action action)
    {
        TickEvent += action;
    }

    public void RemoveEventListener(Action action)
    {
        TickEvent -= action;
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("Can only have one tick manager instance!");
        }
    }
}