using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chaser : MonoBehaviour
{
    public static Chaser instance;

    public enum State { Dormant, Chasing, Waiting3, WaitingReset }
    public State currentState { get; private set; }

    private bool chasing = false;
    Queue<Vector2> PlayerPos = new Queue<Vector2>();

    public SpriteRenderer spr;
    public BoxCollider2D bc;
    public GameObject backLight;
    public float delay = 3;

    private void Awake()
    {
        currentState = State.Dormant;
    }

    public void StartChase()
    {
        StartCoroutine(IStartChase());
    }
    private IEnumerator IStartChase()
    {
        instance = this;
        transform.position = Player.instance.OriginPos;
        currentState = State.Waiting3;
        yield return new WaitForSeconds(delay);
        currentState = State.Chasing;
        spr.enabled = true;
        bc.enabled = true;
        backLight.SetActive(true);
        chasing = true;
    }

    private void Update()
    {
        if (currentState == State.Waiting3)
        {
            PlayerPos.Enqueue(Player.instance.transform.position);
        }
        else if (currentState == State.Chasing)
        {
            PlayerPos.Enqueue(Player.instance.transform.position);
            transform.position = PlayerPos.Dequeue();
        }
    }

    public void Reset()
    {
        PlayerPos.Clear();     

        spr.enabled = false;
        bc.enabled = false;
        backLight.SetActive(false);

        if (currentState != State.Dormant)
            currentState = State.WaitingReset;
    }
}
