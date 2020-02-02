using UnityEngine;
using UnityEngine.UI;

public class AirResourceDisplay : MonoBehaviour
{
    private float levelToDisplay;

    [SerializeField]
    private Material material;

    void Start()
    {
    }
    
    public void FixedUpdate()
    {
        levelToDisplay = UpdateLevelToDisplay();

        material.SetFloat("_Level", levelToDisplay/100.0f);
    }

    protected virtual float UpdateLevelToDisplay()
    {
        if ( !Game.Instance.finishedGeneratingLevel )
        {
            return 0;
        }
        return Game.Instance.Simulation.currentState.airQuality;
    }
}