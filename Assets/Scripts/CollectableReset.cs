﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableReset : ResetableObject
{
    Vector3 originalPos;
    Collectable c;
    private void Awake()
    {
        originalPos = transform.position;
        c = GetComponent<Collectable>();
    }
    public override void Reset()
    {
        if(c.CurrentState == Collectable.State.following)
        {
            transform.parent = null;
            transform.position = originalPos;
            transform.localScale = Vector3.one;
            c.CurrentState = Collectable.State.intact;
            CollectableManager.instance.followingNum = 0;
        }
    }
}
