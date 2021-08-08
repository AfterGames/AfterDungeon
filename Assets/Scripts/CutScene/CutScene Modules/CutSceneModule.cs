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
    [HideInInspector] public UnityEvent OnStart, OnEnd;
    protected CutScene cutScene;

    public void StartCutSceneModule(CutScene cutScene)
    {
        this.cutScene = cutScene;
        StartCutSceneModule();
    }
    protected abstract void StartCutSceneModule();
    protected void CutSceneEnded()
    {
        OnEnd.Invoke();
        cutScene.CutSceneEnded();
    }
}
