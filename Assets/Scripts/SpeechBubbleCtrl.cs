using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubbleCtrl : MonoBehaviour
{
    private RectTransform rt;
    public RectTransform tail;
    public Text content;
    public GameObject tailEnd;
    public HorizontalLayoutGroup hlg;

    public void Awake()
    {
        rt = GetComponent<RectTransform>();
        hlg = GetComponent<HorizontalLayoutGroup>();
    }

    public void SetTail(TailLoc loc)
    {
        float x = 0.5f;
        if (loc == TailLoc.Left)
            x = 0.2f;
        else if (loc == TailLoc.Right)
            x = 0.8f;

        tail.anchorMax = new Vector2(x, 0);
        tail.anchorMin = new Vector2(x, 0);
        rt.pivot = new Vector2(x, 0);
    }

    private char[] txtArray = { };
    public void SetLocation(Vector3 location, ref Canvas c)
    {
        c.renderMode = RenderMode.ScreenSpaceCamera;
        transform.position = location;
        Vector3 v = transform.position - tailEnd.transform.position;
        transform.position += v;
        Vector3 l = transform.localPosition;
        l.z = 0;
        transform.localPosition = l;
        c.renderMode = RenderMode.ScreenSpaceOverlay;
    }
    public void SetText(string text)
    {
        int pad = Screen.width / 40;
        hlg.padding.bottom = pad;
        hlg.padding.top = pad;
        pad = (int) (1.5f * pad);
        hlg.padding.left = pad;
        hlg.padding.right = pad;
        content.text = "";
        content.fontSize = Screen.width / 50;
        //txtArray = text.Replace("NEWLINE", "\n").ToCharArray();
        //currentCharId = 0;
        //talking = true;
        //delayStarted = false;
        content.text = text.Replace("NEWLINE", "\n");
        StartCoroutine(DelayBetweenSpeehces(delayTime));
    }

    private int currentCharId = 0;
    //private float 
    bool delayStarted = false;
    public float delayTime = 0.5f;
    //private void FixedUpdate()
    //{
    //    if (currentCharId < txtArray.Length)
    //    {
    //        content.text += txtArray[currentCharId++];
    //    }

    //    else if (!delayStarted)
    //        StartCoroutine(DelayBetweenSpeehces(delayTime));
    //}

    private bool talking;
    public bool Talking
    {
        get
        {
            //return talking || currentCharId < txtArray.Length;
            return talking;
        }
    }


    IEnumerator DelayBetweenSpeehces(float dt)
    {
        delayStarted = true;
        yield return new WaitForSeconds(dt);
        talking = false;
    }
}
