using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrrigationPanelActivation : MonoBehaviour
{
    public GameObject IrrigationPanelPrefab;
    GameObject IrrigationPanelInstance;

    void OnMouseUpAsButton()
    {
        if (!UiDisable.Instance.disabled)
        {
            Activate();
        }
    }

    public void Activate()
    {
        if (IrrigationPanelInstance != null)
        {
            Destroy(IrrigationPanelInstance);
        }
        else
        {
            UiDisable.Instance.disabled = true;
            IrrigationPanelInstance = Instantiate(IrrigationPanelPrefab);
            IrrigationPanelInstance.GetComponent<IrrigationUI>().Init();
        }
    }
}