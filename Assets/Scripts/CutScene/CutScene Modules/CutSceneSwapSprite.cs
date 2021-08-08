using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneSwapSprite : CutSceneModule
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite targetSprite;

    protected override void StartCutSceneModule()
    {
        StartCoroutine(CoSwapSprite(delay));
    }

    private IEnumerator CoSwapSprite(float delay)
    {
        yield return new WaitForSeconds(delay);
        targetImage.sprite = targetSprite;
    }
}
