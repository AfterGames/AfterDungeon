using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TranslucentMaker : MonoBehaviour
{
    public SpriteRenderer spr;
    private void Start()
    {
        spr.enabled = false;
        StartCoroutine(DelayedMakeTranslucent());
    }

    IEnumerator DelayedMakeTranslucent()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        TranslucentTile tt = Physics2D.OverlapCircle(transform.position, 0.2f).GetComponent<TranslucentTile>();
        tt.MakeTranslucent();
    }
}
