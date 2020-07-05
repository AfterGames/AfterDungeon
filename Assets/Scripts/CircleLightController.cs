using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLightController : MonoBehaviour
{
    public float Brightness;
    public float lightRadius;

    // Start is called before the first frame update
    protected void Start()
    {
        GetComponent<Renderer>().material.SetFloat("_Radius", lightRadius);
        GetComponent<Renderer>().material.SetFloat("_Bright", Brightness);
        GetComponent<Renderer>().material.SetFloat("_CenterX", transform.position.x);
        GetComponent<Renderer>().material.SetFloat("_CenterY", transform.position.y);
        transform.localScale = new Vector3(2 * lightRadius, 2 * lightRadius, 1);
    }

    // Update is called once per frame
    void Update()
    {
        // GetComponent<Renderer>().material.SetFloat("_CenterX", transform.position.x);
        // GetComponent<Renderer>().material.SetFloat("_CenterY", transform.position.y);
    }
}
