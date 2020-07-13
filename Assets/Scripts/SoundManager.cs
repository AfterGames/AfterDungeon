using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bgm;
    public AudioSource sfx;
    public List<AudioClip> sounds;
    public float[] volumes;

    public enum Clip {click1, click2, jump, jumpLand, shoot, walk}

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "0")
            StartBGM();
        instance = this;
    }

    public void Play(Clip clip)
    {
        sfx.clip = sounds[(int)clip];
        sfx.volume = volumes[(int)clip];
        sfx.Play();
    }

    public void StartBGM()
    {
        bgm.enabled = true;
    }
}
