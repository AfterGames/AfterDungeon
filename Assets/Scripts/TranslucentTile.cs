using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TranslucentTile : MonoBehaviour
{
    static Tilemap tm;

    public Color translucentColor;
    Vector3Int tilePos;
    public LayerMask translucentLayer;
    Vector2 boxSize = new Vector2(0.4f, 0.4f);
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            tm.SetColor(tilePos, translucentColor);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            tm.SetColor(tilePos, Color.white);
        }
    }

    public void MakeTranslucent()
    {
        gameObject.layer = 17;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();

        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        bc.size = Vector2.one * 1.125f;
        bc.isTrigger = true;

        if (tm == null)
        {
            tm = FindObjectOfType<Tilemap>();

            var cm = Camera.main.cullingMask;

            if ((cm & translucentLayer) == 0)
            {
                Camera.main.cullingMask = cm | translucentLayer;
            }
        }

        tilePos = tm.WorldToCell(transform.position);
    }
}
