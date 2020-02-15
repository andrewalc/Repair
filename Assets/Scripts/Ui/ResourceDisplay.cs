using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ResourceDisplay : MonoBehaviour
{
    private float levelToDisplay;

    [SerializeField]
    private Material material;
	[SerializeField]
	private TMP_Text valueText;

    protected abstract ResourceType TypeID { get; }

	//Just so we stop having git issues with the damn materials
	private float startFillAmount;

    void Start()
    {
        Game.Instance.OnSimTickFinished += UpdateLevel;

        Game.Instance.ResourceChanged += OnResourceChanged;

		startFillAmount = material.GetFloat("_Level");
    }
    
    public void UpdateLevel(Simulation sim)
    {
        if ( !Game.Instance.finishedGeneratingLevel )
        {
            return;
        }

        levelToDisplay = UpdateLevelToDisplay(sim);

        material.SetFloat("_Level", levelToDisplay/100.0f);
		valueText.text = Mathf.Round(levelToDisplay).ToString();
    }

    private void OnResourceChanged(Simulation sim, float oldValue, ResourceEntry newValue)
    {
        if (newValue.TypeID != TypeID)
        {
            return;
        }
        
        UpdateLevel(sim);
    }

    protected abstract float UpdateLevelToDisplay(Simulation sim);

	private void OnDestroy()
	{
		material.SetFloat("_Level", startFillAmount);
	}
}