using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    AudioSource source;
    public AudioClip intro;
    public AudioClip loop;

    public static BGMPlayer instance;

    bool introPlaying = false;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        instance = this;
        if (FindObjectOfType<BGMStarter>() == null) StartBGM();
    }

    public void StartBGM()
    {
        Debug.Log("브금 재생");
        if (intro != null)
        {
            //   StartCoroutine(PlayIntro());
            PlayIntro();
        }
        else PlayLoop();
    }

    private void Update()
    {
        if(introPlaying)
        {
            if(!source.isPlaying)
            {
                PlayLoop();
            }
        }
    }

    void PlayIntro()
    {
        introPlaying = true;
        source.clip = intro;
        source.Play();

        //yield return new WaitForSeconds(intro.length);
        //Debug.Log("인트로 끝");
        //PlayLoop();
    }

    void PlayLoop()
    {
        introPlaying = false;
        Debug.Log("루프 시작");
        source.loop = true;
        source.clip = loop;
        source.Play();
    }
}
