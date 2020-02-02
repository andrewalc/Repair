using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantHover : MonoBehaviour
{
    void OnMouseOver()
    {
        if (!UiDisable.Instance.disabled)
        {
            PlantState plantState = GetComponent<PlantState>();
            HoverManager.Instance.Display(plantState.square);
        }

    }

    void OnMouseExit()
    {
        HoverManager.Instance.Deactivate();
    }
}
