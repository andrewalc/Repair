using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class CanvasHover : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
