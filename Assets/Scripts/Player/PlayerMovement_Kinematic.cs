using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using DG.Tweening;

public class PlayerMovement_Kinematic : PlayerMovement_parent
{
    public static PlayerMovement_Kinematic instance;
    public Vector2 velocity = Vector2.zero;
    float g = 0;
    [SerializeField] public float gravityScaleFactor;


    public LayerMask dangerousLayer;
    public Vector3 colliderOffset;
    public Vector2 colliderBox;
    public float centerToFrontEnd;

    public Vector3 colliderCenterPosition
    {
        get
        {
            Vector3 co = colliderOffset;
            if (!IsFacingRight) co.x = -co.x;
            return transform.position + co;
        }
    }

    private LayerMask projectileMask;

    public Vector2 originalWallCheckPos;

    protected override void Awake()
    {
        instance = this;
        rb2D = GetComponent<Rigidbody2D>();
        elapsed = 0f;

        //g = originGravity;
        isFired = false;
        isDashing = false;

        BoxCollider2D bc = GetComponent<BoxCollider2D>();

        colliderOffset = bc.offset;
        colliderBox = bc.size + (Vector2.one * 0.1f);
        centerToFrontEnd = bc.offset.x + colliderBox.x / 2;

        originalWallCheckPos = wallChecker.localPosition;
    }

    private void Start()
    {
        whatIsGround += 1 << 16;
        projectileMask = LayerMask.NameToLayer("Projectile");
    }

    bool projectileJumping = false;
    protected override void Update()
    {

        VelocityLimit();
        if(!isDashing && !projectileJumping)
            isGrounded = GroundChecking();
        if (isGrounded) rising = false;

        closestWall = WallChecking();
        g = GravityControl() * gravityScaleFactor;
        if (!isDashing)
            velocity.y -= g * Time.fixedDeltaTime;
        //if (velocity.y > 0)

        Block(ref velocity);
        //if(velocity.y != 0)   Debug.Log("after g"+velocity);
        if (rising) velocity.y = 4;
        if(!Player.instance.dead)
        transform.Translate(velocity * Time.deltaTime);
    }

    private void Block(ref Vector2 velocity)
    {
        ContactPlayer cp = null;
        if (lastGround != null && !isJumping)
        {
            cp = lastGround.GetComponent<ContactPlayer>();
        }
        if (closestWall != null)
        {
            if (IsFacingRight && velocity.x > 0) velocity.x = 0;
            else if (!IsFacingRight && velocity.x < 0) velocity.x = 0;
            while (Physics2D.OverlapBoxAll(penetrateChecker.position, wallBox, 0, whatIsWall).Length > 0)
            {
                transform.Translate(Vector3.right * (IsFacingRight ? -0.1f : 0.1f));
            }
        }
        if(cp == null)
        {
            if (velocity.y < 0 && IsGrounded)
            {
                velocity.y = 0;
                while (Physics2D.OverlapBoxAll(buryChecker.position, buryCheckBox, 0, whatIsGround).Length > 0)
                {
                    transform.Translate(Vector3.up * 0.05f);
                    //Debug.Log("파묻힌 거 꺼내는 중");
                }
            }
            else if (velocity.y > 0)
            {
                if (CeilingCheck())
                {
                    velocity.y = 0;
                }
            }
        }
        
        else if (velocity.y <= 0 || cp.currentVelocity.y >= 0)
        {
            velocity += cp.currentVelocity;
            if(velocity.y < cp.currentVelocity.y)
            {
                velocity.y = cp.currentVelocity.y;
                while (Physics2D.OverlapBoxAll(buryChecker.position, buryCheckBox, 0, whatIsGround).Length > 0)
                {
                    transform.Translate(Vector3.up * 0.05f);
                }
            }
        }
        if (!Player.instance.dead)
        {
            var dangerousObjects = Physics2D.OverlapBoxAll(transform.position + colliderOffset, colliderBox, 0, dangerousLayer);
            if (dangerousObjects.Length > 0)
            {
                SoundManager.instance.Play(SoundManager.Clip.spike);
                Player.instance.GetDamage();
            }
        }

    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundChecker.position, groundBox);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(/*wallChecker.position*/ WallCheckPos, wallBox);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(/*fireChecker.position*/ FireCheckPos, fireBox);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(buryChecker.position, buryCheckBox);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(penetrateChecker.position, wallBox);
    }

    protected override void VelocityLimit()
    {
        if (wallState == WallState.Slide && velocity.y < slidingVelocity)
            velocity.y = slidingVelocity;
        if (velocity.y < (-1) * jumpVelocity.y + 0.2392f * g / 6)
        {
            velocity = new Vector2(velocity.x, (-1) * jumpVelocity.y + 0.2392f * g / 6);
        }

    }

    protected override bool GroundChecking()
    {
        // 상승 중에는 점프 불가
        if (velocity.y >= 0.01f)
        {
            //return false;
        }

        List<Collider2D> colliders = new List<Collider2D>();

        Collider2D[] colls = Physics2D.OverlapBoxAll(groundChecker.position, groundBox, 0, whatIsGround);
        foreach (Collider2D coll in colls)
        {
            colliders.Add(coll);
            //Debug.Log(coll.gameObject.name);
        }

        for (int i = 0; i < colliders.Count; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if (colliders[i].gameObject.layer == LayerMask.NameToLayer("HalfPlatform") && GetComponent<Rigidbody2D>().velocity.y > 0.1f)
                    continue;
                if (colliders[i].gameObject.layer != LayerMask.NameToLayer("MovingPlatform"))
                {
                    platformVelocity = new Vector2(0f, 0f);
                    addVelocity = new Vector2(0f, 0f);
                }

                if (colliders[i].gameObject != lastGround)
                {
                    GroundChange(colliders[i].gameObject);
                }

                lastGround = colliders[i].gameObject;
                lastGroundedTime = Time.time;
                break;
            }

        }

        if (Time.time - lastGroundedTime <= mildJumpTime)
        {
            GroundChange(lastGround);
            if (isGrounded == false) GroundingEvent();
            //projumped = false;
            return true;
        }
        else
        {
            GroundChange(null);
            SetBool("IsGrounded", false);
            return false;
        }
    }

    private bool CeilingCheck()
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(ceilingChecker.position, groundBox, 0, whatIsCeiling);
        for (int i = 0; i < colls.Length; i++)
        {
            Debug.Log(colls[i].gameObject.name);
        }
        return colls.Length > 0;
    }

    private void StretchGroundcheckers(bool stretch)
    {
        float cpIncrment = 0.25f;
        if(stretch)
        {
            groundBox.y += cpIncrment;
            buryCheckBox.y += cpIncrment/2;
        }    
        else
        {
            groundBox.y -= cpIncrment;
            buryCheckBox.y -= cpIncrment / 2;
        }
    }
    
    public override void GroundChange(GameObject Ground)
    {
        if (Ground == null && lastGround != null)
        {
            if (lastGround.GetComponent<ContactPlayer>() != null)
            {
                lastGround.GetComponent<ContactPlayer>().OnPlayerExit(this);
                StretchGroundcheckers(false);
            }
        }
        else if (Ground != null && lastGround == null)
        {
            if (Ground.GetComponent<ContactPlayer>() != null)
            {
                Ground.GetComponent<ContactPlayer>().OnPlayerEnter(this);
                StretchGroundcheckers(true);
            }
        }
        else if (Ground != null && lastGround != null)
        {
            if (Ground == lastGround)
            {
                if (lastGround.GetComponent<ContactPlayer>() != null)

                    lastGround.GetComponent<ContactPlayer>().OnPlayerStay(this);
            }
            else
            {
                if (lastGround.GetComponent<ContactPlayer>() != null)
                {
                    lastGround.GetComponent<ContactPlayer>().OnPlayerExit(this);
                    StretchGroundcheckers(false);
                }
                if (Ground.GetComponent<ContactPlayer>() != null)
                {
                    Ground.GetComponent<ContactPlayer>().OnPlayerEnter(this);
                    StretchGroundcheckers(true);
                }
            }
        }
        lastGround = Ground;
    }

    protected override int? WallChecking(float preOffset = 0)
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(WallCheckPos + Vector2.right * preOffset, wallBox, 0, whatIsWall);
        if (colls.Length != 0)
        {
            List<Collider2D> colliders = new List<Collider2D>();

            foreach (Collider2D coll in colls)
            {
                colliders.Add(coll);
            }
            for (int i = 0; i < colliders.Count; i++)
            {

                if (colliders[i].gameObject != gameObject)
                {
                    if (colliders[i].GetComponent<ContactPlayer>() != null && wallState != WallState.None)
                    {
                        colliders[i].GetComponent<ContactPlayer>().OnWallEnter(this);
                    }
                }
            }
            lastWallTime = Time.time;
            return IsFacingRight ? 1 : -1;
        }

        if (Time.time - lastWallTime <= mildWallTime) return closestWall;
        else return null;
    }

    protected override bool FireChecking()
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(FireCheckPos, fireBox, 0, whatIsWall);
        if (colls.Length != 0)
        {
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].gameObject.GetComponent<ContactArrow>() != null)
                {

                    colls[i].gameObject.GetComponent<ContactArrow>().OnLodgingEnterAction(null);

                    break;
                }
            }
            return false;
        }
        else
            return true;
    }

    protected override bool ContactChecking()
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(FireCheckPos, fireBox, 0, whatIsWall);
        if (colls.Length != 0)
        {
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].gameObject.GetComponent<ContactArrow>() != null)
                {
                    return true;
                }
            }
        }
        return false;

    }

    protected override void GroundingEvent()
    {
        SoundManager.instance.Play(SoundManager.Clip.jumpLand);
        SetBool("IsGrounded", true);
        SetBool("IsJumping", false);
        Stamina = totalStamina;
        isDashed = false;
        //wallChecker.position = originalWallCheckPos;
    }

    protected override float GravityControl()
    {
        if (isGravityControlled) return g;
        //if (!isGrounded && velocity.y > 0 && !isJumpTrue && !projumped) return originGravity * 3f;
        if (wallState == WallState.Slide) return originGravity * wallGravityFactor;
        return originGravity;
    }

    public override void Move(float horizontal, bool jump, bool dash, bool fire)
    {
        //isJumpTrue = jumpdown;
        if (isJumping) horizontal = 0f;
        if (jump) lastJumpInputTime = Time.time;
        if (isGrounded && (velocity.y <= 0 || isPlatform))
        {
            SetBool("IsJumping", false); // 애니메이션 추가
        }

        GrabWall(horizontal);
        HorizontalMove(horizontal);
        if (AllowToJump()) JumpingMovement(horizontal);
        if (wallState == WallState.None)
        {
            if (dash) Dash(horizontal);
            if (fire) Fire(horizontal);
        }

        SetFloat("Jump Speed", velocity.y);
    }

    protected override void JumpingMovement(float horizontal)
    {
        SetBool("IsJumping", true);
        //Debug.Log("Is Ground = " + isGrounded
        //    + " Closest Wall = " + closestWall
        //    + " Wall State = " + wallState
        //    + " Horizontal = " + horizontal);

        if (wallState == WallState.None) Jump(horizontal);
        else if (!isGrounded && closestWall.HasValue) WallJump();
        //Debug.Log(wallState);


        lastJumpInputTime = -999f;
    }

    protected override void Fire(float horizontal)
    {
        
        if (isFired == false && FireChecking())
        {
            Debug.Log("fire");
            SetTrigger("Fire");
            isFired = true;

            //GameObject projectile = Instantiate(projectilePrefab, (transform.position + fireChecker.position) / 2, (transform.localScale.x > 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0)));
            GameObject projectile = Instantiate(projectilePrefab, (transform.position + (Vector3)FireCheckPos) / 2, (transform.localScale.x > 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0)));
            projectile.GetComponent<ProjectileController>().Initialize(IsFacingRight, fireVelocity, maxDistance, this);
            this.projectile = projectile.GetComponent<ProjectileController>();
            this.projectile.SetLimit(projectileTime);
            LoseScarf();

            // StartCoroutine(ShootMotion());

            if (isGrounded == false)
            {
                //projumped = true;
                float x = fireJumpVelocity.x;
                float y = fireJumpVelocity.y;
                if (horizontal > 0) ApplyJumpVelocity(x, y, noSound : true);
                else if (horizontal < 0) ApplyJumpVelocity(-x, y, noSound : true);
                else ApplyJumpVelocity(0, y, noSound: true);
            }
        }
        else if (isFired == false && ContactChecking())
        {
            if (isGrounded == false)
            {
                //projumped = true;
                float x = fireJumpVelocity.x;
                float y = fireJumpVelocity.y;
                if (horizontal > 0) ApplyJumpVelocity(x, y, noSound: true);
                else if (horizontal < 0) ApplyJumpVelocity(-x, y, noSound: true);
                else ApplyJumpVelocity(0, y, noSound: true);
            }
        }
    }

    public void SpringJump()
    {
        Debug.Log("spring jump");
        ApplyJumpVelocity(0, 1.414f * jumpVelocity.y);
    }


    protected override void Dash(float horizontal)
    {
        #region Dash
        if ((isDashed == false) && (isDashing == false)/* && isGrounded == false*/)
        {
            isDashed = true;

            //StartCoroutine(GravityControl(0, dashingTime));

            float x = dashVelocity.x;
            //float y = dashVelocity.y;
            if (IsFacingRight) StartCoroutine(DashMove(x, 0, dashingTime));
            else StartCoroutine(DashMove(-x, 0, dashingTime));
            //StartCoroutine(EndDash());
            //Debug.Log("Stumping : " + velocity);
        }
        #endregion
    }

    private Vector3 dashStartPos;
    protected override IEnumerator DashMove(float x, float y, float dashingTime)
    {
        SoundManager.instance.Play(SoundManager.Clip.dash);
        dashStartPos = transform.position;
        SetTrigger("Dash");
        SetBool("isDashing", true);
        isDashing = true;
        //wallChecker.localPosition += Vector3.right * 0.2f;
        //wallBox.x += 0.2f;
        float startTime = Time.time;
        while ((Time.time - startTime < dashingTime) && !isJumping)
        {
            float elapsed = Time.time - startTime;
            if (WallChecking(velocity.x * Time.deltaTime).HasValue)
                break;
            if (spr.sprite.name == "ch_dashbody2" || spr.sprite.name == "ch_dashbody3")
            {
                Tail.gameObject.SetActive(true);
                Tail.Initiate(dashingTime, transform.localScale.x);
            }
            
            if (elapsed < dashingTime / 4 - Time.deltaTime)
            {
                velocity = new Vector2(elapsed * x * 4 / dashingTime, y);
                Flip(x);
            }
            else if (dashingTime / 4 - Time.deltaTime <= elapsed && elapsed < dashingTime * 3 / 4 - Time.deltaTime)
            {
                velocity = new Vector2(x, y);
                Flip(x);
            }
            else
            {
                velocity = new Vector2((-1) * elapsed * x * 4 / dashingTime + 4 * x, y);
                Flip(x);
            }
            yield return null;
        }
        velocity.x = 0;
        Tail.End(tailPosition.position);
        SetBool("isDashing", false);
        SetTrigger("DashEnd");

        //wallChecker.localPosition = originalWallCheckPos;
        //wallBox.x -= 0.2f;

        isDashing = false;

        if(transform.position.y < dashStartPos.y + float.Epsilon)
        {
            float length = transform.position.x - dashStartPos.x;
            if (Physics2D.OverlapBox(dashStartPos + Vector3.right * length / 2, new Vector2(Mathf.Abs(length), colliderBox.y), 0, projectileMask) != null)
            {
                ProjectileJump();
            }
        }
        SetTrigger("DashEnd");
        if (isGrounded)
        {
            Debug.Log("땅에 닿아서 대시 충전");
            isDashed = false;

        }
    }

    protected override void Jump(float horizontal)
    {

        #region Normal Jump
        if (isGrounded)
        {
            float x = jumpVelocity.x;
            float y = jumpVelocity.y;
            if (horizontal > 0) ApplyJumpVelocity(x, y);
            else if (horizontal < 0) ApplyJumpVelocity(-x, y);
            else ApplyJumpVelocity(0, y);

            //Debug.Log("Normal Jump : " + velocity);
        }
        #endregion
    }

    protected override IEnumerator GravityControl(float value, float duration)
    {
        isGravityControlled = true;
        g = value;

        yield return new WaitForSeconds(duration);

        isGravityControlled = false;
    }

    protected override void WallJump()
    {
        wallState = WallState.None;
        SetTrigger("WallJump");
        SetBool("Wall", false);
        if (closestWall == 1)
        {
            closestWall = null;
            ApplyJumpVelocity(-slidingJumpVelocity.x, slidingJumpVelocity.y, wallJumpExtortionTime);
        }
        else if (closestWall == -1)
        {
            closestWall = null;
            Debug.Log("wall jump");
            ApplyJumpVelocity(slidingJumpVelocity.x, slidingJumpVelocity.y, wallJumpExtortionTime);
        }
        //Debug.Log("Wall Jump : " + velocity);

        lastWallTime = -999f;


    }

    // Control only horizontal velocity
    protected override void HorizontalMove(float dir)
    {
        if (isJumping) return;

        if ((wallState == WallState.Slide || wallState == WallState.upSlide) && IsFacingRight != dir > 0)
        {
            elapsed += Time.deltaTime;
            if (elapsed < detachWallTime)
            {
                return;
            }
            else
            {
                closestWall = null;
                // if(wallState != WallState.None)
                //    Debug.Log("반대방향 눌러서 벽 떨어짐");
                wallState = WallState.None;
                Flip(dir);
                SetBool("Wall", false);
                return;
            }

        }
        if (wallState != WallState.None) return;
        Flip(dir);

        float nowV = velocity.x;

        float targetV = dir * horizontalSpeed + platformVelocity.x;

        if (nowV == targetV) return;

        if ((targetV > nowV) == (nowV > 0))
        {
            if (targetV > nowV)
            {
                nowV += Acceleration * Time.fixedDeltaTime;
                if (nowV > targetV) nowV = targetV;
            }
            else if (targetV < nowV)
            {
                nowV -= Acceleration * Time.fixedDeltaTime;
                if (nowV < targetV) nowV = targetV;
            }
        }
        else
        {
            if (targetV > nowV)
            {
                nowV += Deceleration * Time.fixedDeltaTime;
                if (nowV > targetV) nowV = targetV;
            }
            else if (targetV < nowV)
            {
                nowV -= Deceleration * Time.fixedDeltaTime;
                if (nowV < targetV) nowV = targetV;
            }
        }

        velocity = new Vector2(nowV, velocity.y);
        //if(!isDashing)
        //{
        //    if (Mathf.Abs(nowV) > float.Epsilon)
        //        SoundManager.instance.Play(SoundManager.Clip.walk);
        //    else
        //        SoundManager.instance.Stop();
        //}
    }
    protected override void ApplyJumpVelocity(float x, float y, float duration = 0f, bool noSound = false)
    {
        #region MovingPlatform
        //if (isPlatform && movingPlatform.status == Status.wait_jump)// 움직이는 플랫폼 마지막에 멈춰있는 구간동안 관대한 점프 판정
        //{
        //    if (movingPlatform.directionType == Direction.x)
        //    {
        //        platformVelocity.x = (-1) * (movingPlatform.velocity + movingPlatform.extraVelocity) * movingPlatform.direction.x;
        //        x += (-1) * (movingPlatform.velocity + movingPlatform.extraVelocity) * movingPlatform.direction.x;
        //    }
        //    else
        //    {
        //        platformVelocity.y = (-1) * (movingPlatform.velocity + movingPlatform.extraVelocity) * movingPlatform.direction.y;
        //        y += (-1) * (movingPlatform.velocity + movingPlatform.extraVelocity) * movingPlatform.direction.y;
        //    }
        //}
        //else if (isPlatform && movingPlatform.status == Status.forward)
        //{
        //    if (movingPlatform.directionType == Direction.x)
        //    {
        //        platformVelocity.x += addVelocity.x;
        //        x += platformVelocity.x;
        //    }
        //    else
        //    {
        //        platformVelocity.y += addVelocity.y;
        //        y += platformVelocity.y;
        //    }
        //}
        #endregion
        //Debug.Log(x + ", " + y);

        if(!noSound)
            SoundManager.instance.Play(SoundManager.Clip.jump);
        GroundChange(null);
        velocity = new Vector2(x, y);
        Flip(x);

        if (duration != 0)
        {
            StartCoroutine(EscapeJumping(duration));
        }
    }

    public override void GrabWall(float horizontal)
    {
        bool? goRight = null;
        if (horizontal > 0) goRight = true;
        else if (horizontal < 0) goRight = false;


        if (isGrounded && velocity.y <= 0)
        {
            wallState = WallState.None;
            SetBool("Wall", false);
        }
        else if (closestWall.HasValue /*&& (goRight == isFacingRight) && velocity.y <= 0*/)
        {
            //velocity = new Vector2(0, -slidingVelocity);
            if (wallState == WallState.None && velocity.y <= 0 && (goRight == isFacingRight))
            {
                isWallRight = goRight;
                elapsed = 0;
                velocity = new Vector2(0, 0);
                wallState = WallState.Slide;
                SetBool("Wall", true);
                SoundManager.instance.Play(SoundManager.Clip.wallSlide);
            }
            else if (velocity.y > 0 && (goRight == isFacingRight))
            {
                // Debug.Log("goRight: " + goRight + "isFacingRight: " + isFacingRight);
                isWallRight = goRight;
                if (wallState == WallState.None)
                    elapsed = 0;
                wallState = WallState.upSlide;
            }
            else if (wallState == WallState.upSlide && velocity.y <= 0)
            {
                SetBool("Wall", true);
                wallState = WallState.Slide;
                SoundManager.instance.Play(SoundManager.Clip.wallSlide);
            }

            if (goRight == null)
            {
                elapsed = 0;
            }


            // Stamina -= wallSlideStatmina * Time.deltaTime;

        }
        else
        {
            // if (wallState != WallState.None)
            //    Debug.Log("반대방향 눌러서 벽 떨어짐");
            SoundManager.instance.Stop();
            wallState = WallState.None;
            SetBool("Wall", false);
        }

    }

    public override void ProjectileJump()
    {
        Vector3 newPos = projectile.transform.position;
        float distanceToProjectile = newPos.x - colliderCenterPosition.x;

        bool facingWall = WallChecking(distanceToProjectile).HasValue;
        if(facingWall)
        {
            Debug.Log("벽에 박힌 투사체 점프");
            StartCoroutine(DelayedGroundCheckerExpand());
            if (IsFacingRight)
            {
                
                newPos.x = (int)newPos.x - centerToFrontEnd;
            }
            else
            {
                newPos.x = (int)newPos.x + 1 + centerToFrontEnd;
            }
        }
        transform.position = newPos;
        float x = facingWall ? 0 : projJumpVelocity.x;
        float y = projJumpVelocity.y;
        //Debug.Log("projectile jump x: " + x + " y: " + y);
        Debug.Log("projectile jump");
        if (IsFacingRight) ApplyJumpVelocity(x, y, 0.01f);
        else ApplyJumpVelocity(-x, y, 0.01f);

    }

    float originalX;
    IEnumerator DelayedGroundCheckerExpand()
    {
        projectileJumping = true;
        originalX = groundBox.x;
        groundBox.x = 0.01f;
        yield return new WaitForSeconds(0.2f);
        groundBox.x = originalX;
        projectileJumping = false;
    }

    protected override void Flip(float dir)
    {
        if (Mathf.Abs(dir) < 0.2f) return;
        if (dir > 0 == IsFacingRight) return;

        /*
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        */
        if (dir > 0)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (dir < 0)
        {
            isFacingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    protected override IEnumerator EscapeJumping(float duration)
    {
        g = 0;
        isJumping = true;


        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            if (isGrounded && velocity.y <= 0)
            {
                Debug.Log("ground break!");
                break;
            }
            yield return null;
        }
        isJumping = false;
    }

    public override IEnumerator JumperAccelChange()
    {
        float originAirAccel = airAccel;

        airAccel = 0;

        yield return new WaitForSeconds(0.16f);

        airAccel = originAirAccel;
    }

    public override void Stop()
    {
        velocity = Vector2.zero;
    }

    protected override void SetBool(string name, bool number)
    {
        animator.SetBool(name, number);
        animator2.SetBool(name, number);
    }

    public void AddVelocity(Vector2 vel)
    {
        Debug.Log(velocity);
        velocity += vel;
        Debug.Log(vel);
        Debug.Log(velocity);
    }

    public bool rising;
}