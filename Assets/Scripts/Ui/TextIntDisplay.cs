using UnityEngine;
using UnityEngine.UI;

public abstract class TextIntDisplay : MonoBehaviour
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

        amountToDisplay = GetAmount();

        textToUpdate.text = amountToDisplay.ToString();
    }

    protected abstract int GetAmount();
}