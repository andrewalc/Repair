using UnityEngine;
using UnityEngine.UI;

public abstract class ResourceDisplay : MonoBehaviour
{
    private float levelToDisplay;

    [SerializeField]
    private Material material;

    void Start()
    {
        Tick.Instance.AddEventListener(UpdateLevel);
    }
    
    public void UpdateLevel()
    {
        if ( !Game.Instance.finishedGeneratingLevel )
        {
            return;
        }

        levelToDisplay = UpdateLevelToDisplay();

        material.SetFloat("_Level", levelToDisplay/100.0f);
    }

    protected abstract float UpdateLevelToDisplay();
}