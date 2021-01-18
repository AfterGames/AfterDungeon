using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBacklight : CircleLightController
{

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.localScale = new Vector3(4 * lightRadius * transform.parent.localScale.x, 4 * lightRadius, 1);
        transform.localScale = new Vector3(30 * transform.parent.localScale.x, 30, 1);
        myRenderer.material.SetFloat("_CenterX", transform.position.x);
        myRenderer.material.SetFloat("_CenterY", transform.position.y);
        myRenderer.material.SetFloat("_Radius", lightRadius);
    }

    public void Reduce()
    {
        //GetComponent<Renderer>().material.SetFloat("_Radius", GemStoneManager.instance.reducedRadius);
        lightRadius = GemStoneManager.instance.reducedRadius;
    }

    public void Reset()
    {
        //GetComponent<Renderer>().material.SetFloat("_Radius", GemStoneManager.instance.originalRadius);
        lightRadius = GemStoneManager.instance.originalRadius;
    }
}
