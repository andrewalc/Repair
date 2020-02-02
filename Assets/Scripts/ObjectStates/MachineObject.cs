using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineObject : MonoBehaviour
{
    public GridSquare square;

    public string type;
    public GameObject hydroObject;

    public GameObject aeroObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (square != null)
        {
            MachineCarObject machineCarObject = (MachineCarObject) square.ContainedObject;
            if (machineCarObject.MachineType == MachineCarObject.MachineTypes.Aero)
            {
                type = "Aero";
                aeroObject.SetActive(true);
            } else if (machineCarObject.MachineType == MachineCarObject.MachineTypes.Hydro)
            {
                type = "Hydro";
                hydroObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("No matching machine type");
            }
         }
    }
    
}
