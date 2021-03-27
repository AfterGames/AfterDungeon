using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingReset : ResetableObject
{
    Attachable attachable;
    FallingBlock fallingBlock;
    Animator anim;
    private void Awake()
    {
        attachable = GetComponent<Attachable>();
        fallingBlock = GetComponent<FallingBlock>();
        anim = GetComponent<Animator>();
    }
    public override void Reset()
    {
        fallingBlock.Reset();
        //Debug.Log("reset");
    }
}
