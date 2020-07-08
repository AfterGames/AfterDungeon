using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    private new Light light;
    float originalIntensity;
    private void Awake()
    {
        light = GetComponent<Light>();
        originalIntensity = light.intensity;
    }

    float period = 5f;
    float elapsedTime = 0;
    public float amplitude = 0.25f;
    // Update is called once per frame
    void Update()
    {
        if(elapsedTime >= period)
        {
            elapsedTime = 0;
            period = Random.Range(4f, 6f);
        }
        else
        {
            elapsedTime += Time.deltaTime;
            light.intensity = originalIntensity * (1 + amplitude * Mathf.Sin(2 * Mathf.PI* elapsedTime / period)/2);
        }
    }
}
