using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretcherManagerReset : ResetableObject
{
    public override void Reset()
    {
        StretcherManager.instance.Reset();
    }
}
