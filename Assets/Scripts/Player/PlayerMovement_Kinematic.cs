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
<<<<<<< HEAD
=======
    public static PlayerMovement_Kinematic instance;
>>>>>>> 0da35a0946b146c3ba893b98479e186625ce8e74
    public Vector2 velocity = Vector2.zero;
    float g = 0;
    [SerializeField] public float gravityScaleFactor;

<<<<<<< HEAD
    protected override void Awake()
    {
=======
    public LayerMask dangerousLayer;
    public Vector3 colliderOffset;
    public Vector2 colliderBox;

    protected override void Awake()
    {
        instance = this;
>>>>>>> 0da35a0946b146c3ba893b98479e186625ce8e74
        rb2D = GetComponent<Rigidbody2D>();
        elapsed = 0f;

        //g = originGravity;
        isFired = false;
        isDashing = false;
<<<<<<< HEAD
=======

        BoxCollider2D bc = GetComponent<BoxCollider2D>();

        colliderOffset = bc.offset;
        colliderBox = bc.size + (Vector2.one * 0.1f);
>>>>>>> 0da35a0946b146c3ba893b98479e186625ce8e74
    }

    private void Start()
    {
        whatIsGround += 1 << 16;
    }

<<<<<<< HEAD
    protected override void FixedUpdate()
=======
    protected override void Update()
>>>>>>> 0da35a0946b146c3ba893b98479e186625ce8e74
    {
        VelocityLimit();
        isGrounded = GroundChecking();
        closestWall = WallChecking();
        g = GravityControl() * gravityScaleFactor;
        if(!isDashing)
<<<<<<< HEAD
            velocity.y -= g * Time.fixedDeltaTime;
        //if (velocity.y > 0)

        Block(ref velocity);
        //if(velocity.y != 0)   Debug.Log("after g"+velocity);
        transform.Translate(velocity * Time.deltaTime);
    }

    private void Block(ref Vector2 velocity)
=======
            velocity.y -= g * Time.deltaTime;
        //if (velocity.y > 0)

        Block();
        transform.Translate(velocity * Time.deltaTime);
    }

    private void Block()
>>>>>>> 0da35a0946b146c3ba893b98479e186625ce8e74
    {

        if (closestWall != null)
        {
            if (IsFacingRight && velocity.x > 0) velocity.x = 0;
            else if (!IsFacingRight && velocity.x < 0) velocity.x = 0;
            while (Physics2D.OverlapBoxAll(penetrateChecker.position, wallBox, 0, whatIsWall).Length > 0)
            {
                transform.Translate(Vector3.right * (IsFacingRight? -0.1f : 0.1f));                 
            }
        }
        if (velocity.y < 0 && IsGrounded)
        {
            velocity.y = 0;
            while(Physics2D.OverlapBoxAll(buryChecker.position, buryCheckBox, 0, whatIsGround).Length > 0)
            {
                transform.Translate(Vector3.up * 0.1f);
                //Debug.Log("파묻힌 거 꺼내는 중");
            }
        }
        else if (velocity.y > 0)
        {
            if (CeilingCheck())
            {
                velocity.y = 0;
                Debug.Log("천장에 막힘");
<<<<<<< HEAD
            }
=======

                //if (IsGrounded)
                //{
                //    Player.instance.GetDamage();
                //    Debug.Log("압사");
                //}
            }
        }   

        if(lastGround != null && !isJumping)
        {
            ContactPlayer cp = lastGround.GetComponent<ContactPlayer>();
            if (cp != null && (velocity.y <= 0 || cp.currentVelocity.y >= 0))
            {

                velocity += cp.currentVelocity;
            }
        }

        var dangerousObjects = Physics2D.OverlapBoxAll(transform.position + colliderOffset, colliderBox, 0, dangerousLayer);
        Debug.Log(dangerousObjects.Length);
        if (dangerousObjects.Length > 0)
        {
            Player.instance.GetDamage();
>>>>>>> 0da35a0946b146c3ba893b98479e186625ce8e74
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
        if (wallState == WallState.Slide)
            velocity.y =  slidingVelocity;
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

    public override void GroundChange(GameObject Ground)
    {
        if (Ground == null && lastGround != null)
        {
            if (lastGround.GetComponent<ContactPlayer>() != null)
<<<<<<< HEAD
                lastGround.GetComponent<ContactPlayer>().OnPlayerExit(gameObject);
=======
                lastGround.GetComponent<ContactPlayer>().OnPlayerExit(this);
>>>>>>> 0da35a0946b146c3ba893b98479e186625ce8e74
        }
        else if (Ground != null && lastGround == null)
        {
            if (Ground.GetComponent<ContactPlayer>() != null)
<<<<<<< HEAD
                Ground.GetComponent<ContactPlayer>().OnPlayerEnter(gameObject);
=======
                Ground.GetComponent<ContactPlayer>().OnPlayerEnter(this);
>>>>>>> 0da35a0946b146c3ba893b98479e186625ce8e74
        }
        else if (Ground != null && lastGround != null)
        {
            if (Ground == lastGround)
            {
                if (lastGround.GetComponent<ContactPlayer>() != null)
<<<<<<< HEAD
                    lastGround.GetComponent<ContactPlayer>().OnPlayerStay(gameObject);
=======
                    lastGround.GetComponent<ContactPlayer>().OnPlayerStay(this);
>>>>>>> 0da35a0946b146c3ba893b98479e186625ce8e74
            }
            else
            {
                if (lastGround.GetComponent<ContactPlayer>() != null)
<<<<<<< HEAD
                    lastGround.GetComponent<ContactPlayer>().OnPlayerExit(gameObject);
                if (Ground.GetComponent<ContactPlayer>() != null)
                    Ground.GetComponent<ContactPlayer>().OnPlayerEnter(gameObject);
=======
                    lastGround.GetComponent<ContactPlayer>().OnPlayerExit(this);
                if (Ground.GetComponent<ContactPlayer>() != null)
                    Ground.GetComponent<ContactPlayer>().OnPlayerEnter(this);
>>>>>>> 0da35a0946b146c3ba893b98479e186625ce8e74
            }
        }
        lastGround = Ground;
    }

    protected override int? WallChecking()
    {
        Collider2D[] colls = Physics2D.OverlapBoxAll(WallCheckPos, wallBox, 0, whatIsWall);
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
<<<<<<< HEAD
                        colliders[i].GetComponent<ContactPlayer>().OnWallEnter(this.gameObject);
=======
                        colliders[i].GetComponent<ContactPlayer>().OnWallEnter(this);
>>>>>>> 0da35a0946b146c3ba893b98479e186625ce8e74
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
        Debug.Log("Is Ground = " + isGrounded
            + " Closest Wall = " + closestWall
            + " Wall State = " + wallState
            + " Horizontal = " + horizontal);

        if (wallState == WallState.None) Jump(horizontal);
        else if (!isGrounded && closestWall.HasValue) WallJump();
        Debug.Log(wallState);


        lastJumpInputTime = -999f;
    }

    protected override void Fire(float horizontal)
    {
        if (isFired == false && FireChecking())
        {
            SoundManager.instance.Play(SoundManager.Clip.shoot);
            SetTrigger("Fire");
            isFired = true;

            //GameObject projectile = Instantiate(projectilePrefab, (transform.position + fireChecker.position) / 2, (transform.localScale.x > 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0)));
            GameObject projectile = Instantiate(projectilePrefab, (transform.position + (Vector3) FireCheckPos) / 2, (transform.localScale.x > 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0)));
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
                if (horizontal > 0) ApplyJumpVelocity(x, y);
                else if (horizontal < 0) ApplyJumpVelocity(-x, y);
                else ApplyJumpVelocity(0, y);
            }
        }
        else if (isFired == false && ContactChecking())
        {
            if (isGrounded == false)
            {
                //projumped = true;
                float x = fireJumpVelocity.x;
                float y = fireJumpVelocity.y;
                if (horizontal > 0) ApplyJumpVelocity(x, y);
                else if (horizontal < 0) ApplyJumpVelocity(-x, y);
                else ApplyJumpVelocity(0, y);
            }
        }
    }

<<<<<<< HEAD
=======
    public void SpringJump()
    {
        ApplyJumpVelocity(0, 1.414f * jumpVelocity.y);
    }

>>>>>>> 0da35a0946b146c3ba893b98479e186625ce8e74

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
            Debug.Log("Stumping : " + velocity);
        }
        #endregion
    }
    protected override IEnumerator DashMove(float x, float y, float dashingTime)
    {
        SetTrigger("Dash");
        SetBool("isDashing", true);
        float startTime = Time.time;
        while ((Time.time - startTime < dashingTime) && !isJumping && !WallChecking().HasValue)
        {
            if (spr.sprite.name == "ch_dashbody2" || spr.sprite.name == "ch_dashbody3")
            {
                Tail.gameObject.SetActive(true);
                Tail.Initiate(dashingTime, transform.localScale.x);
            }
            float elapsed = Time.time - startTime;
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
        Tail.End(tailPosition.position);
        SetBool("isDashing", false);

        isDashing = false;
        if (isGrounded)
            isDashed = false;
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

            Debug.Log("Normal Jump : " + velocity);
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
            ApplyJumpVelocity(slidingJumpVelocity.x, slidingJumpVelocity.y, wallJumpExtortionTime);
        }
        Debug.Log("Wall Jump : " + velocity);

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
    }

    protected override void ApplyJumpVelocity(float x, float y, float duration = 0f)
    {
        #region MovingPlatform
        if (isPlatform && movingPlatform.status == Status.wait_jump)// 움직이는 플랫폼 마지막에 멈춰있는 구간동안 관대한 점프 판정
        {
            if (movingPlatform.directionType == Direction.x)
            {
                platformVelocity.x = (-1) * (movingPlatform.velocity + movingPlatform.extraVelocity) * movingPlatform.direction.x;
                x += (-1) * (movingPlatform.velocity + movingPlatform.extraVelocity) * movingPlatform.direction.x;
            }
            else
            {
                platformVelocity.y = (-1) * (movingPlatform.velocity + movingPlatform.extraVelocity) * movingPlatform.direction.y;
                y += (-1) * (movingPlatform.velocity + movingPlatform.extraVelocity) * movingPlatform.direction.y;
            }
        }
        else if (isPlatform && movingPlatform.status == Status.forward)
        {
            if (movingPlatform.directionType == Direction.x)
            {
                platformVelocity.x += addVelocity.x;
                x += platformVelocity.x;
            }
            else
            {
                platformVelocity.y += addVelocity.y;
                y += platformVelocity.y;
            }
        }
        #endregion
        Debug.Log(x + ", " + y);
        if (x - 10 > float.Epsilon)
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
            wallState = WallState.None;
            SetBool("Wall", false);
        }

    }

    public override void ProjectileJump()
    {
        //projumped = true;
<<<<<<< HEAD
=======
        transform.position = projectile.transform.position;
>>>>>>> 0da35a0946b146c3ba893b98479e186625ce8e74
        float x = projJumpVelocity.x;
        float y = projJumpVelocity.y;
        Debug.Log("projectile jump x: " + x + " y: " + y);
        if (IsFacingRight) ApplyJumpVelocity(x, y, 0.01f);
        else ApplyJumpVelocity(-x, y, 0.01f);
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

}