using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCtrlReset : ResetableObject
{
    WindCtrl windCtrl;
    private void Awake()
    {
        windCtrl = GetComponent<WindCtrl>();
    }
    public override void Reset()
    {
        windCtrl.Reset();
    }
}
