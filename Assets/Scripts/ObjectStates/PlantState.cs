using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class PlantState : MonoBehaviour
{
    public string State => GetState();
    public GridSquare square;
    private MeshRenderer mesh;

    public StringToMaterial[] materialMap;
	private Dictionary<string, Material> materialDict = new Dictionary<string, Material>();

	private bool listenersSet = false;

    [System.Serializable] 
    public class StringToMaterial
    {
        public string name;
        public Material material;
    }

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
		foreach(StringToMaterial stm in materialMap)
		{
			materialDict[stm.name] = stm.material;
		}
    }

	public void Update()
	{
		if (!listenersSet)
		{
			Tick.Instance.RemoveEventListener(UpdateHealth);
			Tick.Instance.AddEventListener(UpdateHealth);
			listenersSet = true;
		}
	}

	public void OnDestroy()
	{
		Tick.Instance.RemoveEventListener(UpdateHealth);
		listenersSet = false;
	}

	string GetState()
	{
		if (square.ContainedObject.Type != CarObjectType.Plant) throw new Exception("PlantState does not have a gridsquare with CarObjectType \"Plant\"");
		var plantObject = (PlantCarObject)square.ContainedObject;
		//TODO: Make plant states stored in the plant object and an enum
		if(plantObject.health < 50)
		{
			return "sad";
		}else if(plantObject.health < 100)
		{
			return "medium";
		}
		else
		{
			return "yeet";
		}
	}

	void UpdateHealth()
	{
		mesh.material = materialDict[State];
	}
}
