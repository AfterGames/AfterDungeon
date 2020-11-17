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

    int tick = 0;
    IEnumerator DelayedMakeTranslucent()
    {
        yield return new WaitForSeconds(1);
        TranslucentTile tt = Physics2D.OverlapCircle(transform.position, 0.2f).GetComponent<TranslucentTile>();
        if(tt!=null)
        tt.MakeTranslucent();
    }
}
