using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CutSceneColorChange : CutSceneModule
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Color targetColor;

    protected override void StartCutSceneModule()
    {
        targetImage.DOColor(targetColor, duration).SetEase(ease).SetDelay(delay).OnPlay(() => OnStart.Invoke()).OnComplete(() => CutSceneEnded());
    }
}
