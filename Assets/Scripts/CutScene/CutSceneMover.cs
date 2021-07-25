using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CutSceneMover : CutSceneModule
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Vector2 targetAnchoredPosition;

    public override void StartCutSceneModule(CutScene cutScene)
    {
        this.cutScene = cutScene;
        rectTransform.DOAnchorPos(targetAnchoredPosition, duration).SetEase(ease).SetDelay(delay).OnPlay(()=>OnStart.Invoke()).OnComplete(() => CutSceneEnded());      
    }
}
