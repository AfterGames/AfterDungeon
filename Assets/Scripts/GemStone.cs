using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemStone : MonoBehaviour
{
    public GemStoneManager mgrPrefab;

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
            gameObject.SetActive(false);
            GemStoneManager.instance.Reset();
        }
    }
}
