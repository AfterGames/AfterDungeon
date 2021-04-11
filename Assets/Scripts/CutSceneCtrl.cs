using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneCtrl : MonoBehaviour
{
    public Image frontImage;
    public Image backImage;
    public Text text;

    public Sprite[] spritesFor1 = new Sprite[5];
    private float[] spriteTimeFor1 = { 1f, 1f, 1f, 1f, 1f };
    float fadeIOtime = 0.7f;
    int fadeIOstep = 10;
    public void StartCutScene(int index)
    {
        StartCoroutine(ICutScene(index));
    }

    private IEnumerator ICutScene(int index)
    {
        switch(index)
        {
            case 1:
                FullImage();
                backImage.sprite = spritesFor1[0];
                for(int i = 1; i < 5;i++)
                {
                    yield return new WaitForSeconds(spriteTimeFor1[index]);
                    frontImage.sprite = spritesFor1[i];
                    for (float j = 1; j <= fadeIOstep; j++)
                    {
                        frontImage.color = new Color(1, 1, 1, j / fadeIOstep);
                        yield return new WaitForSeconds(fadeIOtime / fadeIOstep);
                    }
                    backImage.sprite = spritesFor1[i];
                    frontImage.color = Color.clear;
                }
                break;
            case 2:

                break;
        }
        //return null;
    }

    private void FullImage()
    {
        frontImage.rectTransform.anchorMin = Vector2.zero;
        backImage.rectTransform.anchorMin = Vector2.zero;
        frontImage.rectTransform.anchorMax = Vector2.one;
        backImage.rectTransform.anchorMax = Vector2.one;

        frontImage.rectTransform.offsetMax = Vector2.zero;
        frontImage.rectTransform.offsetMin = Vector2.zero;

        backImage.rectTransform.offsetMax = Vector2.zero;
        backImage.rectTransform.offsetMin = Vector2.zero;
    }

    private Vector2 anchorMin = new Vector2(0.15f, 0.35f);
    private Vector2 anchorMax = new Vector2(0.85f, 0.9f);
    private void ImageWithText()
    {
        frontImage.rectTransform.anchorMin = anchorMin;
        backImage.rectTransform.anchorMin = anchorMin;
        frontImage.rectTransform.anchorMax = anchorMax;
        backImage.rectTransform.anchorMax = anchorMax;

        frontImage.rectTransform.offsetMax = Vector2.zero;
        frontImage.rectTransform.offsetMin = Vector2.zero;

        backImage.rectTransform.offsetMax = Vector2.zero;
        backImage.rectTransform.offsetMin = Vector2.zero;
    }
}
