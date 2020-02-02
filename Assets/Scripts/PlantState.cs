using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PlantState : MonoBehaviour
{
    public string State;
    private MeshRenderer mesh;

    public StringToMaterial[] materialMap;
    [System.Serializable] 
    
    public class StringToMaterial
    {
        public string name;
        public Material material;
    }

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    void FixedUpdate()
    {
        foreach (StringToMaterial stm in materialMap)
        {
            if (stm.name != State) continue;
            mesh.material = stm.material;
            break;
        }
    }
}
