using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_superposition : MonoBehaviour
{
    AudioSource source;
    public bool[] relevantIndices;

    public bool playing { get { return source.isPlaying; } }
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        if (relevantIndices[0]) source.Play();
    }
    void Start()
    {
        BGMPlayer.instance.AddBGMsuperposition(this);
    }

    public void Stop()
    {
        if (source.isPlaying)
            StartCoroutine(IFadeOut());
    }

    int steps = 10;
    float foTime = 1.5f;
    IEnumerator IFadeOut()
    {
        for(float i = steps; i > 0; i--)
        {
            source.volume = i / steps;
            yield return new WaitForSeconds(foTime / steps);
        }
        source.Stop();
    }

    public void Play()
    {
        if(!source.isPlaying)
            StartCoroutine(IFadeIn());
    }

    IEnumerator IFadeIn()
    {
        for (float i = 1; i <= steps; i++)
        {
            source.volume = i / steps;
            yield return new WaitForSeconds(foTime / steps);
        }
        source.volume = 1;
        source.Play();
    }
}
