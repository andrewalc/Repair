using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TextIntDisplay : MonoBehaviour
{
    private int amountToDisplay;

    [SerializeField]
    private TMP_Text textToUpdate;

    void Start()
    {
        Tick.Instance.AddEventListener(UpdateAmount);
    }

    protected virtual void UpdateAmount()
    {
        print("TICK");
        if (!Game.Instance.finishedGeneratingLevel)
        {
            return;
        }

        amountToDisplay = GetAmount();

        textToUpdate.text = amountToDisplay.ToString();
    }

    protected abstract int GetAmount();
}