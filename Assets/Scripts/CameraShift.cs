using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShift : MonoBehaviour
{
    public float unitShift;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // left click
        if (Input.GetMouseButtonDown(0))
        {
            transform.GetComponent<Animator>().Play("CabinCamForward");
        }
        // right click
        if (Input.GetMouseButtonDown(1))
        {
            transform.GetComponent<Animator>().Play("CabinCamBackward");
        }
    }

    public void shiftForward()
    {
        transform.parent.position = new Vector3(transform.parent.position.x - unitShift, transform.parent.position.y, transform.parent.position.z);
    }
    public void shiftBackward()
    {
        transform.parent.position = new Vector3(transform.parent.position.x + unitShift, transform.parent.position.y, transform.parent.position.z);
    }
}
