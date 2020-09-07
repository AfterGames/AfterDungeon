using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    Vector3 originalPos;
    public enum State { intact, following, collected }
    public State currentState = State.intact;

    private void Start()
    {
        CollectableManager.instance.Collect += (() => { if(currentState == State.following) currentState = State.collected;});
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (currentState == State.intact)
            {
                currentState = State.following;
                transform.parent = Player.instance.transform;
                transform.localPosition = Vector3.left * 2.3f * ++CollectableManager.instance.followingNum;
            }

            else if (currentState == State.collected)
            {
                CollectableManager.instance.AddCollection();
                Destroy(gameObject);
            }
        }
    }

    public float speed;
    private void Update()
    {
        if(currentState == State.collected)
        {
            //Vector3 offset = Player.instance.transform.position - transform.position;
            //transform.Translate(offset * speed * Time.deltaTime);
            transform.localPosition += speed * Vector3.right * Time.deltaTime;
        }
    }
}
