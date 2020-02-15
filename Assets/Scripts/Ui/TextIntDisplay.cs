using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TextIntDisplay : MonoBehaviour
{
    private int amountToDisplay;

    [SerializeField]
    private TMP_Text textToUpdate;

    protected virtual void Start()
    {
        Game.Instance.OnSimTickFinished += UpdateAmount;
    }

    protected virtual void UpdateAmount(Simulation sim)
    {
        if (!Game.Instance.finishedGeneratingLevel)
        {
            return;
        }

        amountToDisplay = GetAmount(sim);

        textToUpdate.text = amountToDisplay.ToString();
    }

    protected abstract int GetAmount(Simulation sim);
}