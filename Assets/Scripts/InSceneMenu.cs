using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InSceneMenu : MonoBehaviour
{
    public GameObject cam;
    
	//This probably shouldn't be here but I don't know where else to put it
	public void Click()
	{
		SoundManager.Instance.PlaySound(SoundNames.click);
	}

    public void Transistion()
    {
        cam.SetActive(false);
        GetComponent<CanvasGroup>()
            .DOFade(0, 0.5f)
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }

    public void Quit()
    {
        Application.Quit();
    }
}
