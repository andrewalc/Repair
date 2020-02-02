using UnityEngine;
using UnityEngine.UI;

public class PlantMatterDisplay : MonoBehaviour
{
    private int amountToDisplay;

    [SerializeField]
    private Text textToUpdate;

    void Start()
    {
        Tick.Instance.AddEventListener(UpdateAmount);
    }

    protected virtual void UpdateAmount()
    {
        if (!Game.Instance.finishedGeneratingLevel)
        {
            return;
        }

        amountToDisplay = (int)(Game.Instance.Simulation.currentState.plantMatter);

        textToUpdate.text = amountToDisplay.ToString();
    }
}