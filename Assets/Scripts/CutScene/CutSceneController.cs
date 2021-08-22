using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneController : MonoBehaviour
{
    FadeIn fadeIn;
    Player player;

    [SerializeField] private CutScene[] cutScenes;

    private void Awake()
    {
        fadeIn = FindObjectOfType<FadeIn>();
        player = FindObjectOfType<Player>();
    }
    private void Start()
    {
        StartCutScene(0);
    }
    
    public void StartCutScene(int index)
    {
        if (player) player.CanControl(false);
        if (fadeIn) fadeIn.SetFade(false);

        cutScenes[index].gameObject.SetActive(true);
    }

    public void OnCutSceneEnd()
    {
        if (player) player.CanControl(true);
        if (fadeIn) fadeIn.SetFade(true);
    }
}