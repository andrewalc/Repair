using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    void Awake()
    {
    }

    public void Display()
    {
        gameObject.SetActive(true);
    }

    public void DisableMyself()
    {
		SoundManager.Instance.PlaySound(SoundNames.click); //hacky;
        Game.Instance.BeginGame();
        gameObject.SetActive(false);
    }
}
