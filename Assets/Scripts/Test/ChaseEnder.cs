using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnder : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(Chaser.instance.gameObject);
        }
    }
}
