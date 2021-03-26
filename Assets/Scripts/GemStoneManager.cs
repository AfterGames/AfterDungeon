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

    //private void Start()
    //{
    //    BGMPlayer.instance.PlaySuperPosition(0);
    //}

    public void Collect()
    {
        switch(++collected)
        {
            case 1:
                if(dark != null)
                    dark.SetActive(true);
                if(pbl != null)
                    pbl.gameObject.SetActive(true);
                break;
            case 2:
                if (pbl != null)
                    pbl.Reduce();
                break;
        }
        if(BGMPlayer.instance != null)
            BGMPlayer.instance.PlaySuperPosition(collected);
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
        BGMPlayer.instance.PlaySuperPosition(0);
    }
}
