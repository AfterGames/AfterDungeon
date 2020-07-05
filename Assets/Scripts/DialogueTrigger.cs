using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().StopMoving();
            Debug.Log("대화 트리거");
            Dialogue.instance.StartTalk();
            Destroy(gameObject);
        }
    }
}
