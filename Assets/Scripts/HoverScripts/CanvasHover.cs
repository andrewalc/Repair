using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class CanvasHover : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.position = Input.mousePosition;
    }
}
