using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BoldChanger : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public Text text;
    public Font normalFont;
    public Font boldFont;

    private void Awake()
    {
        text = transform.GetComponentInChildren<Text>();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        text.font = boldFont;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        text.font = normalFont;
    }
}
