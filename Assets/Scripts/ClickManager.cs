using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = cam.ScreenToWorldPoint(target.position);
        Debug.Log("target is " + screenPos.x + " pixels from the left");
    }
}
