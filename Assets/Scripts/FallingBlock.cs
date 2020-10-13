using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FallingReset))]
public class FallingBlock : ContactPlayer
{
    [SerializeField]private Rigidbody2D rb2D;
    public Vector3 origin;
    public float fallSpeed;

    public bool intact = true;
    public bool isFalling = false;
    public bool fallEnded = false;

    private float elapsed = 0;

    public PlayerMovement_parent curPlayer;


    public bool logger;
    Attachable attachable;
    [SerializeField]FallingBlock father;
    [SerializeField]List<FallingBlock> cluster;
    public Animator animator;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position - Vector3.up * 0.05f, new Vector2(0.8f,0.9f));

        
    }
    private void Start()
    {
        origin = transform.position;

        attachable = GetComponent<Attachable>();
        if (attachable != null)
        {
            if (attachable.allFather != null)
            {
                rb2D = attachable.allFather.GetComponent<Rigidbody2D>();
                father = attachable.allFather.GetComponent<FallingBlock>();
            }
            if (rb2D == null)
                StartCoroutine(WaitFather());
        }
        else
        {
            rb2D = GetComponent<Rigidbody2D>();
            father = attachable.allFather.GetComponent<FallingBlock>();
        }

        if(this == father)
        {
            gameObject.name = "FallingBlockFather";
            cluster = new List<FallingBlock>();
            cluster.Add(this);
            int j = 0;
            for(int i = 0; i < transform.childCount; i++)
            {
                var fb = transform.GetChild(i).GetComponent<FallingBlock>();
                if (fb != null)
                {
                    cluster.Add(fb);
                    fb.gameObject.name = "FallingBlock" + j;
                    j++;
                }
            }
            //if(logger)
        }
    }
    //public bool debug;
    Vector2 prev;
    private void FixedUpdate()
    {
        if(!fallEnded && isFalling)
            Check();

        //if (rb2D != null && currentVelocity.magnitude > Mathf.Epsilon)
        //{
        //    isFalling = true;
        //}
        if (father != null)
        {
            currentVelocity = father.currentVelocity;
        }

        if (currentVelocity.magnitude > Mathf.Epsilon && !fallEnded)
        {
            //isFalling = true;
            prev = transform.position;
            if(father ==this)transform.position = prev + currentVelocity * Time.fixedDeltaTime;
        }
    }

    private IEnumerator WaitFather()
    {
        while (rb2D == null)
        {
            yield return new WaitForSeconds(0.08f);
            if(attachable.allFather !=null)
                rb2D = attachable.allFather.GetComponent<Rigidbody2D>();
                father = attachable.allFather.GetComponent<FallingBlock>(); 
        }

    }

    public override void OnPlayerEnter(PlayerMovement_parent player)
    {
        curPlayer = player;

        if (intact)
        {
            Debug.Log("player enter " + gameObject.name);
            //rb2D.GetComponent<FallingBlock>().curPlayer = player;
            StartCoroutine(father.DelayedFall());
        }
    }
    private IEnumerator DelayedFall()
    {
        if (intact)
        {
            //Debug.Log("delayed fall " + gameObject.name);
            foreach (FallingBlock fb in cluster)
            {
                fb.animator.SetTrigger("Shake");
                fb.intact = false;
                //Debug.Log(fb.intact);
            }
            yield return new WaitForSeconds(1);

            foreach (FallingBlock fb in cluster)
            {
                fb.StartFall();
                fb.animator.SetTrigger("Fall");
            }
        }
        else yield return null;
    }

    private void StartFall()
    {
        if (isFalling || fallEnded) return;
        //Debug.Log("start fall " + gameObject.name);
        isFalling = true;
        currentVelocity = new Vector2(0, -father.fallSpeed);
        //Debug.Log(currentVelocity);
    }


    public override void OnPlayerExit(PlayerMovement_parent player)
    {
        curPlayer = null;
        //player.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity + currentVelocity;
        //player.GetComponent<Rigidbody2D>().isKinematic = false;
        //player.transform.parent = null;

    }

    public override void OnPlayerStay(PlayerMovement_parent player)
    {
    }

    public override void OnWallEnter(PlayerMovement_parent player)
    {
        if (intact)
        {
            curPlayer = player;
            StartCoroutine(father.DelayedFall());
        }
    }

    void Check()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position - Vector3.up * 0.05f, new Vector2(0.9f, 0.9f), 0);
        foreach (Collider2D coll in colls)
        {
            colliders.Add(coll);
        }

        for (int i = 0; i < colliders.Count; i++)
        {
            if (colliders[i].gameObject != gameObject && colliders[i].gameObject.tag!="Player")
            {
                var a = colliders[i].GetComponent<Attachable>();
                if (a != null)
                {
                    if (a.allFather != GetComponent<Attachable>().allFather && a.transform.position.y > transform.position.y)
                    {
                        Debug.Log(a.allFather != GetComponent<Attachable>().allFather);
                        Debug.Log(a.allFather.gameObject.name +", "+GetComponent<Attachable>().allFather.gameObject.name);
                        EndFalling();
                    }
                }

                //else if (colliders[i].tag != "Dangerous")
                //    EndFalling();
                else
                {
                    EndFalling();
                }

                break;
            }
        }
    }

    private void EndFalling()
    {
        if (fallEnded) return;
        if(logger)Debug.Log("end " +gameObject.name);
        currentVelocity = new Vector2(0, 0);
        if(curPlayer != null)
        {

            if (!Player.instance.beKinematic)
            {
                curPlayer.GetComponent<Rigidbody2D>().isKinematic = false;
                curPlayer.transform.parent = null;
            }

        }
        fallEnded = true;

        if (this != father)
            father.EndFalling();
        else
        {
            foreach(FallingBlock fb in cluster)
            {
                if(!fb.fallEnded)    fb.EndFalling();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag=="Player" && Mathf.Abs(collision.collider.transform.position.x-transform.position.x)<0.9f)
        {
            GameObject player = collision.collider.gameObject;
            if(player != null)
            if(player.transform.position.y<transform.position.y-0.7f && player.GetComponent<PlayerMovement_Kinematic>().IsGrounded && isFalling)
            {
                player.GetComponent<Player>().GetDamage();
            }
        }
        else if(collision.collider.tag != "Player")
        {
            Debug.Log("OUCH!");
            if(collision.collider.transform.position.y<transform.position.y-0.2f && currentVelocity.y<0)
            {
                //currentVelocity = new Vector2(0, 0);

                if (curPlayer != null)
                {
                    curPlayer.Stop();
                    if(!Player.instance.beKinematic)
                    {
                        //curPlayer.movingPlatform = null;
                        curPlayer.rb2D.isKinematic = false;

                    }

                }

            }
        }
    }
}
