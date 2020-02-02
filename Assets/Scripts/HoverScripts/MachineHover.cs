using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineHover : MonoBehaviour
{
    void OnMouseOver()
    {
        if (!UiDisable.Instance.disabled)
        {
            MachineObject machineState = GetComponent<MachineObject>();
            HoverManager.Instance.Display(machineState.square); 
        }
    }

    void OnMouseExit()
    {
        HoverManager.Instance.Deactivate();
    }
}
