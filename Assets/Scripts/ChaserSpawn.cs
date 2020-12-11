using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserSpawn : MonoBehaviour
{
    private static ChaserSpawn instance;
    [Header("체크하면 주인공이 닿았을 때 스폰되지 않고 추격이 끝남")]
    public bool end = false;
    BoxCollider2D bc;
    public Chaser myChaser;
    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (instance != null && instance != this) Destroy(instance.gameObject);
            if (end) return;
            instance = this;
            Debug.Log("spawn snake");
            bc.enabled = false;
            myChaser.StartChase();
        }
    }

    public void Reset()
    {
        bc.enabled = true;
        if(myChaser != null)
            myChaser.Reset();
    }
}
