using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemStone : MonoBehaviour
{
    public GemStoneManager mgrPrefab;
    private SpriteRenderer spr;
    private Collider2D col;

    private void Awake()
    {
        if (spr == null) spr = GetComponent<SpriteRenderer>();
        if (col == null) col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        if (GemStoneManager.instance == null)
            Instantiate(mgrPrefab);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GemStoneManager.instance.Collect();
            spr.enabled = false;
            col.enabled = false;
        }
    }
    public void Reset()
    {
        spr.enabled = true;
        col.enabled = true;
    }
}
