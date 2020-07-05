using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBacklight : CircleLightController
{

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(2 * lightRadius * transform.parent.localScale.x, 2 * lightRadius, 1);
        GetComponent<Renderer>().material.SetFloat("_CenterX", transform.position.x);
        GetComponent<Renderer>().material.SetFloat("_CenterY", transform.position.y);

    }

}
