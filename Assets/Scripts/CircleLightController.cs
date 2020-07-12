using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLightController : MonoBehaviour
{
    public float Brightness;
    public float lightRadius;
    [SerializeField] private float curRadius;
    [SerializeField] private bool isBlink;
    [SerializeField] private float blinkRange;

    private int BlinkDirection = 1;

    // Start is called before the first frame update
    protected void Start()
    {
        GetComponent<Renderer>().material.SetFloat("_Radius", lightRadius);
        GetComponent<Renderer>().material.SetFloat("_Bright", Brightness);
        GetComponent<Renderer>().material.SetFloat("_CenterX", transform.position.x);
        GetComponent<Renderer>().material.SetFloat("_CenterY", transform.position.y);
        transform.localScale = new Vector3(2 * lightRadius, 2 * lightRadius, 1);
        curRadius = lightRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBlink)
            Blink();
        // GetComponent<Renderer>().material.SetFloat("_CenterX", transform.position.x);
        // GetComponent<Renderer>().material.SetFloat("_CenterY", transform.position.y);
    }

    protected void Blink()
    {
        curRadius += BlinkDirection * 0.004f;
        if (curRadius > lightRadius + blinkRange)
            BlinkDirection = -1;
        else if (curRadius < lightRadius - blinkRange)
            BlinkDirection = 1;
        GetComponent<Renderer>().material.SetFloat("_Radius", curRadius);
    }
}
