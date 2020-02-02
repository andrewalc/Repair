using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrrigationPanelActivation : MonoBehaviour
{
    public GameObject IrrigationPanelPrefab;
    GameObject IrrigationPanelInstance;
    
    void OnMouseOver(){
        if(!UiDisable.Instance.disabled && Input.GetMouseButtonDown(0))
        {
            if (IrrigationPanelInstance != null) {
                Destroy(IrrigationPanelInstance);
            } else
            {
                UiDisable.Instance.disabled = true;
                IrrigationPanelInstance = Instantiate(IrrigationPanelPrefab);
                IrrigationPanelInstance.GetComponent<IrrigationUI>().Init();
            }
        }
    }
}
