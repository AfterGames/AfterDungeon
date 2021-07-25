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
        if (player) player.canControlSetter = false;
        if (fadeIn) fadeIn.SetFade(false);

        cutScenes[index].gameObject.SetActive(true);
    }

    public void OnCutSceneEnd()
    {
        if (player) player.canControlSetter = true;
        if (fadeIn) fadeIn.SetFade(true);
    }
}