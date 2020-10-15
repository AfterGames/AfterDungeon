using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private bool started = false;
    public PlayableDirector upwards;
    public TimelineAsset tla;
    public Image fader;
    //void Update()
    //{
    //    if(Input.anyKeyDown && !started)
    //    {
    //        started = true;
    //        upwards.Play(tla);
    //    }
    //}
    public void Quit()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        StartCoroutine(FadeOutAndStartGame());
    }
    IEnumerator FadeOutAndStartGame()
    {
        Saver.BICDemoLoad();
        for(int i = 0; i < 5; i++)
        {
            fader.color = new Color(0,0,0, (i + 1) / 5);
            yield return new WaitForSeconds(0.05f);
        }
        SceneManager.LoadScene("BIC_Demo");
    }
}
