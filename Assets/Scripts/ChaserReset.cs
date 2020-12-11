using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserReset : ResetableObject
{
    public ChaserSpawn cs;
    public override void Reset()
    {
        cs.Reset();
    }

    public void OnDestroy()
    {
        resetObjects.Remove(this);
    }
}
