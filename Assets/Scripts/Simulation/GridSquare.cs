using UnityEngine;

public class GridSquare
{
    public int X
    {
        get;
        set;
    }
    
    public int Y
    {
        get;
        set;
    }

    private ICarObject containedObject;
    public ICarObject ContainedObject
    {
        get
        {
            return containedObject;
        }
        set
        {
            if (ContainedObject != null)
            {
                Debug.LogError("Error: Square already contains an object.");
            }

            containedObject = value;
        }
    }
}
