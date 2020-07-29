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
        fallingBlock.intact = true;
        fallingBlock.fallEnded = false;
        fallingBlock.isFalling = false;
        //anim.SetTrigger("fall");
        
        //if(attachable != null)
        //{
        //    GetComponent<Attachable>().allFather.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0f);
        //    GetComponent<Attachable>().allFather.transform.position = GetComponent<Attachable>().allFatherPosition;
        //}

        fallingBlock.currentVelocity = Vector2.zero;
        if(attachable.IsFather)
            transform.position = fallingBlock.origin;
        Debug.Log("reset");
    }
}
