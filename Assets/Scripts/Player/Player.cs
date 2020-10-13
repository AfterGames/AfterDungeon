using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(PlayerMovement_parent))]
public class Player : MonoBehaviour
{
    public bool beKinematic;
    [Header("Control State")]
    public bool fireLock;
    [SerializeField] private bool canControl = true;
    public bool specialControl;
    public static Player instance;

    public PlayerMovement_Kinematic mover { get; private set; }
    [SerializeField]private Animator animator;
    [SerializeField] private Animator animator2;
    private float horizontal = 0;
    private bool jump = false;
    private bool dash = false;
    private bool respawn = false;
    private bool esc = false;

    private bool fire = false;
    private bool stillfire = false;
    private bool fireUp = false;

    private GameObject FadeObject;
    [SerializeField]private GameObject FadeObjectPrefab;

    private float fireButtonTime = 0f;

    private Vector2 originPos;
    public int stageNum;

    private InGameMenu escMenu;
    Rigidbody2D rb;
    public bool dialogueReady = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        fireButtonTime = 0f;
        if (beKinematic)
        {
            mover = GetComponent<PlayerMovement_Kinematic>();
            //Destroy(GetComponent<PlayerMovement>());
            rb.gravityScale = 0;
            //rb.bodyType = RigidbodyType2D.Kinematic;
            //rb.constraints = RigidbodyConstraints2D.FreezeAll ;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            rb.freezeRotation = true;
        }
        else
        {
            //mover = GetComponent<PlayerMovement>();
            Destroy(GetComponent<PlayerMovement_Kinematic>());
        }
        FadeObject = GameObject.FindGameObjectWithTag("FadeObject");
        if(FadeObject==null)
        {
            FadeObject = Instantiate(FadeObjectPrefab, GameObject.FindGameObjectWithTag("MainCamera").transform);
            FadeObject.transform.localPosition = new Vector3(0, 0, 10);
        }
        FadeObject.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        rb = GetComponent<Rigidbody2D>();
        if (tag == "Player") instance = this;
    }

    private void Start()
    {
        if (DataAdmin.instance.GetData(DataType.game_world) >= 0 && DataAdmin.instance.GetData(DataType.game_stage) >= 0)
        {
            SetSpawnPos(FindObjectOfType<SpawnController>().transform.GetChild(DataAdmin.instance.GetData(DataType.game_stage)).GetComponent<SpawnRegion>().spawnPositionObject.transform.position);
            transform.position = FindObjectOfType<SpawnController>().transform.GetChild(DataAdmin.instance.GetData(DataType.game_stage)).GetComponent<SpawnRegion>().spawnPositionObject.transform.position;
        }
        else
            SetSpawnPos(transform.position);
        escMenu = (InGameMenu)FindObjectOfType(typeof(InGameMenu));
    }

    private void Update()
    {
        //치트키 와드
        respawn = Input.GetButtonDown("Respawn");
        esc = Input.GetKeyDown(KeyCode.Escape);
        if (esc && escMenu != null)
        {
           // Debug.Log("esc pressed");
            Time.timeScale = escMenu.isOn? 1:0;
            escMenu.ActivateAll(!escMenu.isOn);
            SoundManager.instance.Play(SoundManager.Clip.esc);
        }
        if(respawn && !specialControl)
        {
            SpawnController.instance.Respawn();
            transform.position = originPos;
            GetFalseDamage(0.5f);
        }
        if (canControl && Time.timeScale>0 && !specialControl)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            jump = Input.GetButtonDown("Jump");
            dash = Input.GetButtonDown("Dash");
            fire = Input.GetButtonDown("Fire");
            stillfire = Input.GetButton("Fire");
            fireUp = Input.GetButtonUp("Fire");
            
            if (stillfire)
            {
                fireButtonTime += Time.deltaTime;
                if (fireButtonTime > 1.0f)
                    mover.SetProjectileTime(1.2f);
            }
        }
        mover.Move(horizontal, jump, dash, fireLock? false : fire);
        if (fireUp)
        {
            mover.SetProjectileTime(fireButtonTime);
            fireButtonTime = 0f;
        }
        
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        animator2.SetFloat("Speed", Mathf.Abs(horizontal));
        if (Mathf.Abs(horizontal) < 0.05f && mover.wallStateP == PlayerMovement_parent.WallState.None)
            SoundManager.instance.Stop();
        else if(mover.IsGrounded)
            SoundManager.instance.Play(SoundManager.Clip.walk);

        jump = false;
        
    }

    public void GetDamage(float duration = 0.8f)
    {
        if (dead) return;
        SoundManager.instance.Stop();
        SoundManager.instance.Play(SoundManager.Clip.gameOver);

        if(!fireLock)
            mover.FireEnd();
        DataAdmin.instance.IncrementData(DataType.deathNum);
        if (!canControl) return;
        canControl = false;

        StartCoroutine(Die(duration));
        StartCoroutine(IFadeOut());
    }
    public void FadeOut()
    {
        Debug.Log("fadeout");
        StartCoroutine(IFadeOut());
    }
    public void FadeInAfterDelay()
    {
        StartCoroutine(IDelayFadeIn());
    }


    public void GetFalseDamage(float duration = 0.8f)
    {
        if (!canControl) return;
        canControl = false;

        StartCoroutine(Die(duration));
    }

    public void SetSpawnPos(Vector2 value, float x = 0, float y = 0, int num = -999)
    {
        Debug.Log("Spawn set: "+ value);
        originPos = value;
        //transform.position = value;
        GetComponent<Rigidbody2D>().velocity = new Vector2(x,y);
        if (num != -999)
            stageNum = num;
    }

    private IEnumerator IFadeOut()
    {
        float rad = 30;

        FadeObject.SetActive(true);
        FadeObject.GetComponent<Renderer>().material.SetFloat("_CenterX", transform.position.x);
        FadeObject.GetComponent<Renderer>().material.SetFloat("_CenterY", transform.position.y);
        while (rad>5)
        {
            FadeObject.GetComponent<Renderer>().material.SetFloat("_Radius", rad);
            rad -= 50*Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        while (rad > 0)
        {
            FadeObject.GetComponent<Renderer>().material.SetFloat("_Radius", rad);
            rad -= 25 * Time.deltaTime;
            yield return null;
        }

        //FadeObject.GetComponent<Renderer>().material.SetFloat("_Radius", 30);
        //FadeObject.SetActive(false);
    }

    private IEnumerator ICompleteFadeOut()
    {
        float rad = 30;

        FadeObject.SetActive(true);
        FadeObject.GetComponent<Renderer>().material.SetFloat("_CenterX", transform.position.x);
        FadeObject.GetComponent<Renderer>().material.SetFloat("_CenterY", transform.position.y);
        while (rad > 0)
        {
            FadeObject.GetComponent<Renderer>().material.SetFloat("_Radius", rad);
            rad -= 60 * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator IFadeIn()
    {
        float rad = 0;
        transform.position = originPos;
        FadeObject.SetActive(true);
        FadeObject.GetComponent<Renderer>().material.SetFloat("_CenterX", transform.position.x);
        FadeObject.GetComponent<Renderer>().material.SetFloat("_CenterY", transform.position.y);

        while (rad < 5)
        {
            FadeObject.GetComponent<Renderer>().material.SetFloat("_Radius", rad);
            rad += 25 * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        while (rad < 50)
        {
            FadeObject.GetComponent<Renderer>().material.SetFloat("_Radius", rad);
            rad += 100 * Time.deltaTime;
            yield return null;
        }

        FadeObject.GetComponent<Renderer>().material.SetFloat("_Radius", 50);
        FadeObject.SetActive(false);
        CanControl(true);
    }

    private IEnumerator IDelayFadeIn()
    {
        specialControl = true;
        transform.position = originPos;
        FadeObject.SetActive(true);
        FadeObject.GetComponent<Renderer>().material.SetFloat("_CenterX", transform.position.x);
        FadeObject.GetComponent<Renderer>().material.SetFloat("_CenterY", transform.position.y);
        FadeObject.GetComponent<Renderer>().material.SetFloat("_Radius", 5);
        int steps = 80;
        for(int i = 0; i < steps; i++)
        {
            FadeObject.GetComponent<Renderer>().material.SetFloat("_CenterX", transform.position.x);
            yield return new WaitForSeconds(1.2f / steps);
        }
        float rad = 5;
        Debug.Log("rad=0");
        while (rad < 100)
        {
            FadeObject.GetComponent<Renderer>().material.SetFloat("_Radius", rad);
            rad += 50 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        FadeObject.SetActive(false);
        specialControl = false;
    }

    public bool dead = false;
    private IEnumerator Die(float duration)
    {
        dead = true;
        gameObject.transform.parent = null;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        animator.SetTrigger("Die");
        animator2.SetTrigger("Die");
        //GetComponent<SpriteRenderer>().DOKill();
        //GetComponent<SpriteRenderer>().color = Color.white;
        CanControl(false);

        yield return new WaitForSeconds(duration);
        ResetableObject.ResetAll();

        //animator.SetBool("Die", false);
        animator.SetTrigger("Respawn");
        animator2.SetTrigger("Respawn");
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        StartCoroutine(IFadeIn());
        //transform.position = originPos; FadeIn으로 이동함
        //CanControl(true); FadeIn으로 이동함
        dead = false;
    }

    public void CanControl(bool canControl)
    {
        this.canControl = canControl;
        if (canControl) specialControl = false;
        horizontal = 0;
        jump = false;
        fire = false;
    }

    public void StopMoving()
    {
        SoundManager.instance.Stop();
        mover.Stop();
        specialControl = true;
        CanControl(false);
        animator.SetFloat("Speed", -1);
    }
}
