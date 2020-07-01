using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubbleCtrl : MonoBehaviour
{
    private RectTransform rt;
    public RectTransform tail;
    public Text content;

    public void Awake()
    {
        rt = GetComponent<RectTransform>();
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
    public void SetText(string text)
    {
        content.text = "";
        txtArray = text.Replace("NEWLINE", "\n").ToCharArray();
        currentCharId = 0;
    }

    private int currentCharId = 0;
    //private float 

    private void FixedUpdate()
    {
        if (currentCharId < txtArray.Length)
        {
            content.text += txtArray[currentCharId++];
        }
    }
}
