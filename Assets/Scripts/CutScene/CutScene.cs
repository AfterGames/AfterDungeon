using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CutScene : MonoBehaviour
{
    CutSceneModule[] cutSceneModules;
    [SerializeField] private UnityEvent OnCutSceneEnd;
    private int endCutSceneCnt = 0;


    private void Awake()
    {
        cutSceneModules = GetComponents<CutSceneModule>();
    }

    private void Start()
    {
        for (int i = 0; i < cutSceneModules.Length; i++)
        {
            cutSceneModules[i].StartCutSceneModule(this);
        }
    }

    public void CutSceneEnded()
    {
        endCutSceneCnt++;
        if (endCutSceneCnt == cutSceneModules.Length)
        {
            OnCutSceneEnd.Invoke();
            gameObject.SetActive(false);
        }
    }
}
