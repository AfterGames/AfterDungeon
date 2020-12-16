using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemStoneReset : ResetableObject
{
    private GemStone gs;

    private void Awake()
    {
        gs = GetComponent<GemStone>();
    }

    public override void Reset()
    {
        gameObject.SetActive(true);
        GemStoneManager.instance.Reset();
    }
}
