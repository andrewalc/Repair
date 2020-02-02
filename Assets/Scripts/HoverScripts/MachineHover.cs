using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineHover : MonoBehaviour
{
    void OnMouseOver()
    {
        MachineObject machineState = GetComponent<MachineObject>();
        HoverManager.Instance.Display(machineState.square);
    }

    void OnMouseExit()
    {
        HoverManager.Instance.Deactivate();
    }
}
