using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    void Awake()
    {
        DisableMyself();
    }

    public void Display()
    {
        gameObject.SetActive(true);
    }

    public void DisableMyself()
    {
        gameObject.SetActive(false);
    }
}
