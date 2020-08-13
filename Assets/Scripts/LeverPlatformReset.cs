using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeverPlatformReset : ResetableObject
{
    LeverPlatform lp;
    [SerializeField]bool A = true;

    private void Awake()
    {
        lp = GetComponent<LeverPlatform>();
        A = GetComponent<SpriteRenderer>().sprite.name.Contains( "LeverPlatformA");
        Reset();
    }
    public override void Reset()
    {
        if(A)
        {
            lp.Activate();
            Debug.Log("activate a");
        }
        else
        {
            lp.Deactivate();
            Debug.Log("deactivate b");
        }
    }
}
