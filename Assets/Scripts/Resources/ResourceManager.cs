using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public WaterResource water;
    // Start is called before the first frame update
    void Start()
    {
        water.value = 10;
        water.rate = -1;
        water.max = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
        water.Tick();
        print(water.value);
    }
}
