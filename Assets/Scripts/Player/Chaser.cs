using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chaser : MonoBehaviour
{
    //[Header("Control State")]
    //public bool fireLock;
    //[SerializeField] private bool canControl = true;
    //public bool specialControl;

    //public PlayerMovement_Kinematic mover { get; private set; }
    //[SerializeField] private Animator animator;
    //private float horizontal = 0;
    //private bool jump = false;
    //private bool dash = false;

    //private bool fire = false;
    //private bool stillfire = false;
    //private bool fireUp = false;

    //private float fireButtonTime = 0f;

    //private Vector2 originPos;
    //public int stageNum;

    //Rigidbody2D rb;
    //public bool dialogueReady = false;


    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    fireButtonTime = 0f;
    //    mover = GetComponent<PlayerMovement_Kinematic>();
    //    //Destroy(GetComponent<PlayerMovement>());
    //    rb.gravityScale = 0;
    //    //rb.bodyType = RigidbodyType2D.Kinematic;
    //    //rb.constraints = RigidbodyConstraints2D.FreezeAll ;
    //    rb.constraints = RigidbodyConstraints2D.FreezePositionY;

    //    rb = GetComponent<Rigidbody2D>();
    //    rb = GetComponent<Rigidbody2D>();
    //}

    //private void Start()
    //{

    //}

    //private void Update()
    //{
    //    //치트키 와드
    //    if (canControl && Time.timeScale > 0 && !specialControl)
    //    {
    //        horizontal = Input.GetAxisRaw("Horizontal");
    //        jump = Input.GetButtonDown("Jump");
    //        dash = Input.GetButtonDown("Dash");
    //        fire = Input.GetButtonDown("Fire");
    //        stillfire = Input.GetButton("Fire");
    //        fireUp = Input.GetButtonUp("Fire");

    //        if (stillfire)
    //        {
    //            fireButtonTime += Time.deltaTime;
    //            if (fireButtonTime > 1.0f)
    //                mover.SetProjectileTime(1.2f);
    //        }
    //    }
    //    mover.Move(horizontal, jump, dash, fireLock ? false : fire);
    //    if (fireUp)
    //    {
    //        mover.SetProjectileTime(fireButtonTime);
    //        fireButtonTime = 0f;
    //    }

    //    animator.SetFloat("Speed", Mathf.Abs(horizontal));
    //    if (Mathf.Abs(horizontal) < 0.05f && mover.wallStateP == PlayerMovement_parent.WallState.None)
    //        SoundManager.instance.Stop();
    //    else if (mover.IsGrounded)
    //        SoundManager.instance.Play(SoundManager.Clip.walk);

    //    jump = false;

    //}

    //IEnumerator DelayedJump()
    //{
    //    yield return new WaitForSeconds(3);
    //}


    //public void CanControl(bool canControl)
    //{
    //    this.canControl = canControl;
    //    if (canControl) specialControl = false;
    //    horizontal = 0;
    //    jump = false;
    //    fire = false;
    //}

    //public void StopMoving()
    //{
    //    SoundManager.instance.Stop();
    //    mover.Stop();
    //    specialControl = true;
    //    CanControl(false);
    //    animator.SetFloat("Speed", -1);
    //}

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
        instance = this;
    }

    public void StartChase()
    {
        StartCoroutine(IStartChase());
    }
    private IEnumerator IStartChase()
    {
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
