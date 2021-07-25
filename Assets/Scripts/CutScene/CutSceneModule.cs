using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public abstract class CutSceneModule : MonoBehaviour
{
    [SerializeField] protected float delay;
    [SerializeField] protected float duration;
    [SerializeField] protected Ease ease;
    [SerializeField] protected UnityEvent OnStart, OnEnd;
    protected CutScene cutScene;

    public abstract void StartCutSceneModule(CutScene cutScene);
    protected void CutSceneEnded()
    {
        OnEnd.Invoke();
        cutScene.CutSceneEnded();
    }
}
