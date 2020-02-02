using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantHover : MonoBehaviour
{
    void OnMouseOver()
    {
        PlantState plantState = GetComponent<PlantState>();
        HoverManager.Instance.Display(plantState.square);
    }

    void OnMouseExit()
    {
        HoverManager.Instance.Deactivate();
    }
}
