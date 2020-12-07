using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserSpawn : MonoBehaviour
{
    public static ChaserSpawn instance;
    BoxCollider2D bc;
    private void Awake()
    {
        instance = this;
        bc = GetComponent<BoxCollider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("spawn snake");
            bc.enabled = false;
            Chaser.instance.StartChase();
        }
    }

    public void Reset()
    {
        bc.enabled = true;
    }
}
