using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InSceneMenu : MonoBehaviour
{
    public GameObject cam;
    

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
