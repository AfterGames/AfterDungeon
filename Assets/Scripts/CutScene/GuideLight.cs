using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideLight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float maximumDistance, minimumDistance;
    private Transform playerT;

    private void Awake()
    {
        playerT = FindObjectOfType<Player>().transform;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float distanceFromPlayer = transform.position.x - playerT.position.x;
        float progress = distanceFromPlayer - minimumDistance;
        progress = Mathf.Clamp01(progress / (maximumDistance - minimumDistance));
        spriteRenderer.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, progress);
    }
}
