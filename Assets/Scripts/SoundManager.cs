using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource sfx;
    public SoundDictionary soundDictionary;
    [Header("볼륨이 1이 아닌 클립들을 넣고 볼륨을 입력해주세요")]
    public VolumeDictionary volumeDictionary;

    public AudioSource spikeSound;
    public AudioSource continuous;

    public enum Clip {esc, menuCursor, jump, jumpLand, shoot, wallSlide, getGem, dash, gameOver, spike, dashGemRegen, dialogue, mouseSqueak, sizzle, walk }

    private void Awake()
    {
        instance = this;
        spikeSound.volume = volumeDictionary[Clip.spike];
    }

    public void Play(Clip clip)
    {
        if(clip == Clip.spike)
        {
            spikeSound.Play();
        }
        else if(clip == Clip.walk || clip == Clip.wallSlide || clip == Clip.sizzle)
        {
            if (soundDictionary[clip] == continuous.clip && continuous.isPlaying)
            {
                //Debug.Log("이미 재생중 " + clip);
                return;
            }
            continuous.clip = soundDictionary[clip];
            continuous.volume = volumeDictionary[clip];
            continuous.Play();
        }
        else
        {
            sfx.Stop();
            sfx.clip = soundDictionary[clip];
            //if (volumeDictionary.ContainsKey(clip))
            //{
            //    sfx.volume = volumeDictionary[clip];
            //}
            //else
            //{
                sfx.volume = 1;
            //}
            sfx.Play();
            
        }
    }

    public void Stop()
    {
        if (!continuous.isPlaying) return;
        continuous.Stop();
    }

    public void Sizzle()
    {
        Play(Clip.sizzle);
    }
}
