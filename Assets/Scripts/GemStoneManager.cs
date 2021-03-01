using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemStoneManager : MonoBehaviour
{
    private int collected = 0;
    public static GemStoneManager instance;
    private GameObject dark;
    private PlayerBacklight pbl;

    public float originalRadius;
    public float reducedRadius;

    private void Awake()
    {
        instance = this;
        Reset();
    }

    public void Collect()
    {
        switch(++collected)
        {
            case 1:
                dark.SetActive(true);
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
        if (dark == null)
        {
            dark = GameObject.Find("DarkEffect");
            Debug.Log(dark);
        }
        if (pbl == null)
        {
            pbl = FindObjectOfType<PlayerBacklight>();
            Debug.Log(pbl);
        }
        if(dark != null) dark.SetActive(false);
        if (pbl != null)
        {
            pbl.gameObject.SetActive(false);
            pbl.Reset();
        }
    }
}
