using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    Vector3 originalPos;
    public enum State { intact, following, collected }
    private State currentState = State.intact;
    public State CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            currentState = value;
            bc.enabled = currentState != State.following;
            if (currentState == State.following) source.clip = get;
            else if (currentState == State.intact) source.clip = touch;
        }
    }
    BoxCollider2D bc;
    AudioSource source;
    public AudioClip get;
    public AudioClip touch;

    private void Start()
    {
        CollectableManager.instance.Collect += (() => { if(currentState == State.following) currentState = State.collected;});
        bc = GetComponent<BoxCollider2D>();
        source = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            source.Play();
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
