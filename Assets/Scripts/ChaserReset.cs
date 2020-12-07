using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserReset : ResetableObject
{
    public override void Reset()
    {
        Chaser.instance.Reset();
        ChaserSpawn.instance.Reset();
    }
}
