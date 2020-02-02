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
    
    public void AnimateForward()
    {
        transform.GetComponent<Animator>().Play("CabinCamForward");
    }

    public void AnimateBackward()
    {
        transform.GetComponent<Animator>().Play("CabinCamBackward");
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
