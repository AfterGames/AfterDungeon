using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemStoneManager : MonoBehaviour
{
    private int collected = 0;
    public static GemStoneManager instance;
    public GameObject dark;
    public PlayerBacklight pbl; 

    private void Awake()
    {
        instance = this;
        Reset();
        pbl = FindObjectOfType<PlayerBacklight>();
    }

    public void Collect()
    {
        switch(++collected)
        {
            case 1:
                dark.SetActive(false);
                pbl.gameObject.SetActive(true);
                break;
            case 2:
                pbl.Reduce();
                break;
        }
    }

    public void Reset()
    {
        collected = 0;
        dark.SetActive(true);
        pbl.gameObject.SetActive(false);
        pbl.Reset();
    }
}
