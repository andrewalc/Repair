using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrrigationPanelActivation : MonoBehaviour
{
    public GameObject IrrigationPanelPrefab;
    GameObject IrrigationPanelInstance;
    
    void OnMouseOver(){
        if(Input.GetMouseButtonDown(0))
        {
            if (IrrigationPanelInstance != null) {
                Destroy(IrrigationPanelInstance);
            } else {
                IrrigationPanelInstance = Instantiate(IrrigationPanelPrefab);
                IrrigationPanelInstance.GetComponent<IrrigationUI>().Init();
            }
        }
    }
}
