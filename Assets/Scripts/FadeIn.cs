using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    SpriteRenderer sr;
    int tick = 100;
    int initialTick;

    private bool startFadeIn = true;

    public void SetFade(bool fadeIn)
    {
        startFadeIn = fadeIn;

        tick = initialTick;
        SetFadeAmount();

        if (!fadeIn)
            gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        initialTick = tick;
    }


    private void Update()
    {
        if (!startFadeIn) return;

        if(--tick > 0)
        {
            SetFadeAmount();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void SetFadeAmount()
    {
        sr.color = new Color(0, 0, 0, (float)tick / initialTick);
    }
}
