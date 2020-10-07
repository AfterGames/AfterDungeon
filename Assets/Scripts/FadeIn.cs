using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    SpriteRenderer sr;
    int tick = 100;
    int initialTick;

    // Start is called before the first frame update
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        initialTick = tick;
    }


    private void Update()
    {
        if(--tick > 0)
        {
            sr.color = new Color(0, 0, 0, ((float)tick / initialTick));
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
