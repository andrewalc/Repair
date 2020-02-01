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
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit ray;
            if (Physics.Raycast(cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)), out ray))
            {
                Instantiate(thing, ray.point, Quaternion.identity);
            }
        }
    }
}
