using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    //단순 반복, 인트로 재생후 반복, 전환, 겹치기
    AudioSource source;
    public AudioClip[] once;
    public AudioClip[] loop;
    public bool PlayOnAwake;

    public static BGMPlayer instance;


    List<BGM_superposition> bgmSuperposList = new List<BGM_superposition>();
    public void AddBGMsuperposition(BGM_superposition bgms)
    {
        bgmSuperposList.Add(bgms);
    }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        instance = this;
        if (PlayOnAwake) PlayBGM(0);
    }

    public void PlayBGM(int index)
    {
        source.Stop();
        source.volume = 1;
        Debug.Log("브금 재생");
        if(!source.isPlaying)
        {
            if (once[index] != null)
            {
                PlayIntro(index);
            }
            else PlayLoop(index);
        }
        
        else
        {
            StartCoroutine(IFadeOut(index));
        }
    }

    void PlayIntro(int index)
    {
        introCoroutine = StartCoroutine(IPlayIntro(index));
    }

    Coroutine introCoroutine;
    IEnumerator IPlayIntro(int index)
    {
        source.loop = false;
        source.clip = once[index];
        source.Play();
        yield return new WaitUntil(() => !source.isPlaying);
        PlayLoop(index);
    }

    void PlayLoop(int index)
    {
        Debug.Log("루프 시작");
        source.loop = true;
        source.clip = loop[index];
        source.Play();
    }

    int steps = 10;
    public float fadeOutTime = 0.7f;
    IEnumerator IFadeOut(int index)
    {
        if (introCoroutine != null)
        {
            StopCoroutine(introCoroutine);
        }
        for (float i = steps; i > 0; i--)
        {
            source.volume = i / steps;
            yield return new WaitForSeconds(fadeOutTime / steps);
        }

        source.Stop();

        PlayBGM(index);
    }

    public void PlaySuperPosition(int index)
    { 
        for(int i = 0; i < bgmSuperposList.Count; i++)
        {
            if (bgmSuperposList[i].relevantIndices[index])
                bgmSuperposList[i].Play();

            else
                bgmSuperposList[i].Stop();
        }
    }

}