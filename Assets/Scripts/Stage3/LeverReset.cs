using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverReset : ResetableObject
{
    Lever lever;

    private void Awake()
    {
        lever = GetComponent<Lever>();
        
    }

    public override void Reset()
    {
        lever.Reset();
    }
}
