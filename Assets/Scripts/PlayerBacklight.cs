﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBacklight : CircleLightController
{

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localScale = new Vector3(4 * lightRadius * transform.parent.localScale.x, 4 * lightRadius, 1);
        GetComponent<Renderer>().material.SetFloat("_CenterX", transform.position.x);
        GetComponent<Renderer>().material.SetFloat("_CenterY", transform.position.y);

    }

}
