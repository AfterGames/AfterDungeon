using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM_superposition : MonoBehaviour
{
    AudioSource source;
    public bool[] relevantIndices;
    [Range(0.0f, 1.0f)]
    public float volume;

    public bool playing { get { return source.isPlaying; } }
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        if (relevantIndices[0]) source.Play();
    }
    void Start()
    {
        BGMPlayer.instance.AddBGMsuperposition(this);
        if (BGMPlayer.instance.PlayOnAwake) source.Play();
    }

    public void FadeOut()
    {
        StartCoroutine(IFadeOut());
    }

    int steps = 10;
    float foTime = 1.5f;
    IEnumerator IFadeOut()
    {
        for(float i = steps; i > 0; i--)
        {
            source.volume = volume * i / steps;
            yield return new WaitForSeconds(foTime / steps);
        }
        source.volume = 0;
        //source.Stop();
    }
    public void Play()
    {
        source.Play();
    }
    public void FadeIn()
    {
        StartCoroutine(IFadeIn());
    }

    IEnumerator IFadeIn()
    {
        for (float i = 1; i <= steps; i++)
        {
            source.volume = volume * i / steps;
            yield return new WaitForSeconds(foTime / steps);
        }
        source.volume = volume;
        //source.Play();
    }
}
