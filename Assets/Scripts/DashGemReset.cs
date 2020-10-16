using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashGemReset : ResetableObject
{
    DashGem dg;
    private void Awake()
    {
        dg = GetComponent<DashGem>();
    }

    public override void Reset()
    {
        dg.EnableByReset();
    }
}
