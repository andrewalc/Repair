using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    public GameObject thing;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // middle click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)), out hit))
            {
//                hit.transform.parent.GetComponent<>()
            }
        }
    }
}
