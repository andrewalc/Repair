using System;
using UnityEngine;

public class Tick : MonoBehaviour
{
    private float _timePerTick;
    private float _timeUntilTick;
    private event Action TickEvent;
    
    public static Tick Instance { get; private set; }

    private void Start()
    {
        _timePerTick = 2;
        _timeUntilTick = _timePerTick;
    }

    public void FixedUpdate()
    {
        _timeUntilTick -= Time.deltaTime;

        if (_timeUntilTick < 0)
        {
            _timeUntilTick += _timePerTick;
            if (TickEvent != null)
            {
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
            Debug.LogError("Can only have one game instance!");
        }
    }
}