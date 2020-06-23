using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingReset : ResetableObject
{

    public override void Reset()
    {
        GetComponent<FallingBlock>().isFalling = false;
        if(GetComponent<Attachable>() != null)
        {
            GetComponent<Attachable>().allFather.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0f);
            GetComponent<Attachable>().allFather.transform.position = GetComponent<Attachable>().allFatherPosition;
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        transform.position = GetComponent<FallingBlock>().origin;

    }
}
