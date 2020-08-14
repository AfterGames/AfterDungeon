using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using DG.Tweening;

public abstract class PlayerMovement_parent : MonoBehaviour
{
    [Header("Current State")]
    [SerializeField] protected bool isGrounded;
    [SerializeField] protected bool isJumping;
    [SerializeField] protected bool isDashed;
    [SerializeField] protected bool isFired;
    [SerializeField] public bool isPlatform;
    public SpriteRenderer spr;
    public SpriteRenderer spr2;
    //protected bool projumped;


    [HideInInspector] public MovingPlatform movingPlatform;
    [HideInInspector] public Vector2 platformVelocity;
    [HideInInspector] public Vector2 addVelocity;

    protected bool isDashing;
    [SerializeField] protected WallState wallState;

    [Header("Basic Movement")]
    [Tooltip("지면 수평 가속도")]
    [SerializeField] protected float groundAccel;
    [Tooltip("지면 수평 감속도")]
    [SerializeField] protected float groundDecel;
    [Tooltip("공중 수평 가속도")]
    [SerializeField] protected float airAccel;
    [Tooltip("공중 수평 감속도")]
    [SerializeField] protected float airDecel;
    [Tooltip("기본 수평 이동속도")]
    [SerializeField] protected float horizontalSpeed;
    [Tooltip("기본 중력")]
    [SerializeField] protected float originGravity;

    [Header("Jump Movement")]
    [Tooltip("점프 속도 (오른쪽 점프 기준)")]
    [SerializeField] protected Vector2 jumpVelocity;
    [Tooltip("대쉬 속도 (오른쪽 대쉬 기준)")]
    [SerializeField] protected Vector2 dashVelocity;
    [Tooltip("대쉬시 떨어지는 꼬리")]
    [SerializeField] protected TailController Tail;
    [SerializeField] protected Transform tailPosition;
    [Tooltip("지면으로 인정할 Layer")]
    [SerializeField] protected LayerMask whatIsGround;
    [Tooltip("지면 존재 유무를 판정할 위치")]
    [SerializeField] protected Transform groundChecker;
    [SerializeField] protected Transform buryChecker;
    
    [SerializeField] protected Transform ceilingChecker;
    [SerializeField] protected LayerMask whatIsCeiling;

    [Header("Fire Movement")]
    [SerializeField] protected Transform fireChecker;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float maxDistance;
    [SerializeField] protected float fireVelocity;
    [SerializeField] protected Vector2 fireJumpVelocity;
    [SerializeField] protected Vector2 projJumpVelocity;
    [Tooltip("발사 버튼 누른 시간이 짧았을 때 화살 존재 시간")]
    [SerializeField] protected float shortFireTime;
    [Tooltip("발사 버튼 누른 시간이 길었을 때 화살 존재 시간")]
    [SerializeField] protected float longFireTime;
    protected ProjectileController projectile;
    protected float projectileTime = 1.5f;

    public enum WallState { None, Slide, upSlide }
    [Header("Wall Movement")]
    [Tooltip("벽으로 인정할 Layer")]
    [SerializeField] protected LayerMask whatIsWall;
    [SerializeField] protected Transform wallChecker;
    [Tooltip("벽에서 미끄러질 속도")]
    [SerializeField] protected float slidingVelocity;
    [Tooltip("벽에서 미끄러질 때 중력 배수")]
    [SerializeField] protected float wallGravityFactor;
    [Tooltip("벽에서 떨어지는데 필요한 입력 시간")]
    [SerializeField] protected float detachWallTime;
    [Tooltip("벽점프 속도 (오른쪽 점프 기준)")]
    [SerializeField] protected Vector2 slidingJumpVelocity;
    [SerializeField] protected Transform penetrateChecker;

    protected bool? isWallRight = null;
    protected float elapsed;

    [Header("Stamina")]
    [SerializeField] protected float totalStamina;
    [SerializeField] protected float stamina;
    [SerializeField] protected float wallSlideStatmina;

    [Header("Fine Control")]
    [Tooltip("지면을 벗어났을 때 유효 점프 인정 시간")]
    [SerializeField] protected float mildJumpTime;
    [Tooltip("이른 점프 입력을 했을 때 유효한 인풋 판정 시간")]
    [SerializeField] protected float allowedJumpTime;
    [Tooltip("대쉬 지속 시간. 이 시간동안 중력이 0이 되고 다른 조작 불가")]
    [SerializeField] protected float dashingTime;
    [Tooltip("벽을 벗어난 이후에도 벽 점프를 할 수 있는 시간")]
    [SerializeField] protected float mildWallTime;
    [Tooltip("벽점프 이후 조작 강탈 시간")]
    [SerializeField] protected float wallJumpExtortionTime;


    protected Vector2 groundBox = new Vector2(0.7f, 0.2f);
    protected Vector2 buryCheckBox = new Vector2(0.7f, 0.1f);
    protected Vector2 wallBox = new Vector2(0.2f, 1.2f);
    protected Vector2 penetrateBox = new Vector2(0.2f, 1.2f);
    protected Vector2 fireBox = new Vector3(1f, 0.4f);

    public Rigidbody2D rb2D { get; protected set; }
    [SerializeField] protected Animator animator;
    [SerializeField] protected Animator animator2;

    protected bool isFacingRight = true;
    protected bool isGravityControlled;
    protected float lastGroundedTime = -100;                              // 너그러운 점프를 위한 마지막으로 지면에 있던 시간
    protected float lastJumpInputTime = -999;                             // 이른 점프 인풋을 위한 마지막으로 점프를 누른 시점
    protected float lastWallTime = -999f;                                 // 너그러운 벽점프를 위한 마지막 벽 인접 시간
    [SerializeField]protected int? closestWall = null;


    protected int num = 0;
    //protected bool isJumpTrue = false;


    public bool IsGrounded { get { return isGrounded; } }
    public bool IsFacingRight { get { return isFacingRight; } }

    protected GameObject lastGround;

    public float Acceleration
    {
        get
        {
            if (isGrounded) return groundAccel;
            else return airAccel;
        }
    }

    public float Deceleration
    {
        get
        {
            if (isGrounded) return groundDecel;
            else return airDecel;
        }
    }

    public Vector2 WallCheckPos
    {
        get
        {

            //if (isFacingRight) return transform.position + wallChecker.localPosition;
            //else return transform.position - wallChecker.localPosition;

            return wallChecker.position;
        }
    }

    public Vector2 FireCheckPos
    {
        get
        {

            //if (isFacingRight) return transform.position + fireChecker.localPosition;
            //else return transform.position - fireChecker.localPosition;

            return fireChecker.position;

        }
    }


    protected abstract void Awake();



    protected abstract void Update();

    protected abstract void OnDrawGizmos();

    protected abstract void VelocityLimit();

    protected abstract bool GroundChecking();

    public abstract void GroundChange(GameObject Ground);

    protected abstract int? WallChecking();

    protected abstract bool FireChecking();

    protected abstract bool ContactChecking();

    protected abstract void GroundingEvent();

    protected abstract float GravityControl();

    public abstract void Move(float horizontal, bool jump, bool dash, bool fire);

    protected abstract void JumpingMovement(float horizontal);

    protected abstract void Fire(float horizontal);

    protected abstract void Dash(float horizontal);

    protected abstract IEnumerator DashMove(float x, float y, float dashingTime);

    protected abstract void Jump(float horizontal);

    protected abstract IEnumerator GravityControl(float value, float duration);

    protected abstract void WallJump();

    // Control only horizontal velocity
    protected abstract void HorizontalMove(float dir);

    protected abstract void ApplyJumpVelocity(float x, float y, float duration = 0f);

    public abstract void GrabWall(float horizontal);

    public abstract void ProjectileJump();

    public abstract void Stop();

    protected float Stamina
    {
        get { return stamina; }
        set
        {
            float target = value;
            if (target < 25 && stamina >= 25)
            {
                //GetComponent<SpriteRenderer>().DOColor(new Color(1, 0.5f, 0.5f), 0.15f)
                //   .SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
            }
            else if (target >= 25)
            {
                // GetComponent<SpriteRenderer>().DOKill();
                //GetComponent<SpriteRenderer>().color = Color.white;
            }

            stamina = target;
        }
    }

    public bool AllowToJump()
    {
        if (Time.time - lastJumpInputTime <= allowedJumpTime) return true;
        else return false;
    }

    protected abstract void Flip(float dir);

    protected abstract IEnumerator EscapeJumping(float duration);

    public abstract IEnumerator JumperAccelChange();

    public void SetProjectileTime(float pressedTime)
    {

        if (pressedTime <= 1.0f)
        {
            projectileTime = shortFireTime;
        }
        else
        {
            projectileTime = longFireTime;
        }

        if (projectile != null)
        {
            projectile.SetLimit(projectileTime);
        }
    }

    public void DashRefill()
    {
        if (isDashed)
        {
            isDashed = false;
            SetTrigger("DashEnd");
        }
    }

    public void FireEnd()
    {
        isFired = false;
        if (projectile != null)
        {
            Destroy(projectile.gameObject);
        }
        GetScarf();
    }

    public void GetScarf()
    {
        animator.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        animator2.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        if (Player.instance != null) if (Player.instance.fireLock) Player.instance.fireLock = false;
    }

    public void LoseScarf()
    {
        animator2.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        animator.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    protected void SetFloat(string name, float number)
    {
        animator.SetFloat(name, number);
        animator2.SetFloat(name, number);
    }
    protected abstract void SetBool(string name, bool number);
    protected void SetTrigger(string name)
    {
        animator.SetTrigger(name);
        animator2.SetTrigger(name);
    }

}