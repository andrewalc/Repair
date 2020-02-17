using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TextIntDisplay : MonoBehaviour
{
    protected int amountToDisplay;

    [SerializeField]
    protected TMP_Text textToUpdate;

    protected virtual void Start()
    {
        Game.Instance.OnSimTickFinished += UpdateAmount;

        if (null != Game.Instance.Simulation)
        {
            UpdateAmount(Game.Instance.Simulation);
        }
    }

    protected virtual void UpdateAmount(Simulation sim)
    {
        if (!Game.Instance.FinishedGeneratingLevel)
        {
            return;
        }

        amountToDisplay = GetAmount(sim);

        textToUpdate.text = amountToDisplay.ToString();
    }

    protected abstract int GetAmount(Simulation sim);
}