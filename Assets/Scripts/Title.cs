using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Title : MonoBehaviour
{
    private bool started = false;
    public PlayableDirector upwards;
    public TimelineAsset tla;
    void Update()
    {
        if(Input.anyKeyDown && !started)
        {
            started = true;
            upwards.Play(tla);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
}
