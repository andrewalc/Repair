using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShift : MonoBehaviour
{
    public float shiftX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            print("right click");
            transform.GetComponent<Animator>().Play("CabinCam");
        }
    }

    public void shiftPosition()
    {
        transform.parent.position = new Vector3(transform.parent.position.x + shiftX, transform.parent.position.y, transform.parent.position.z);
    }
}
