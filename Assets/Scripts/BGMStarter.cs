using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMStarter : MonoBehaviour
{
    public int index = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            BGMPlayer.instance.PlayBGM(index);
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
}
