using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public AudioSource source;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision");
        if(collision.tag == "Player")
        {
            source.Play();
            //Debug.Log("spring and player collision");
            var mover = collision.GetComponent<PlayerMovement_Kinematic>();
            if(mover.velocity.y < 0)
            {
                mover.SpringJump();
            }
        }
    }
}
