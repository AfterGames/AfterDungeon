using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FallingReset))]
public class FallingBlock : ContactPlayer
{
    [SerializeField]private Rigidbody2D rb2D;
    public Vector3 origin;
    public float velocity;

    public bool isFalling = false;

    private float elapsed = 0;

    public GameObject curPlayer;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(0.8f,1f));

        
    }
    private void Start()
    {

        origin = transform.position;
        if (GetComponent<Attachable>() != null)
        {
            if (GetComponent<Attachable>().allFather != null)
            {
                rb2D = GetComponent<Attachable>().allFather.GetComponent<Rigidbody2D>();
            }
            if (rb2D == null)
                StartCoroutine(WaitFather());
        }
        else
            rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Check();

        if (rb2D != null && rb2D.velocity.magnitude > Mathf.Epsilon)
        {
            isFalling = true;
        }

        

    }

    private IEnumerator WaitFather()
    {
        while (rb2D == null)
        {
            yield return new WaitForSeconds(0.08f);
            if(GetComponent<Attachable>().allFather!=null)
                rb2D = GetComponent<Attachable>().allFather.GetComponent<Rigidbody2D>();
        }

    }

    public override void OnPlayerEnter(GameObject player)
    {
        if(!isFalling)
        {
            rb2D.GetComponent<FallingBlock>().curPlayer = player;
            isFalling = true;           
            rb2D.velocity = new Vector2(0, -rb2D.GetComponent<FallingBlock>().velocity);
            //player.GetComponent<Rigidbody2D>().isKinematic = true;
            //player.transform.parent = rb2D.transform;

        }
    }

    public override void OnPlayerExit(GameObject player)
    {
        rb2D.GetComponent<FallingBlock>().curPlayer = null;
        //player.GetComponent<Rigidbody2D>().velocity = player.GetComponent<Rigidbody2D>().velocity + rb2D.velocity;
        //player.GetComponent<Rigidbody2D>().isKinematic = false;
        //player.transform.parent = null;

    }

    public override void OnPlayerStay(GameObject player)
    {
    }

    public override void OnWallEnter(GameObject player)
    {
        if (!isFalling)
        {
            rb2D.GetComponent<FallingBlock>().curPlayer = player;
            isFalling = true;
            rb2D.velocity = new Vector2(0, -rb2D.GetComponent<FallingBlock>().velocity);
        }
    }

    void Check()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.9f, 1f), 0);
        foreach (Collider2D coll in colls)
        {
            colliders.Add(coll);
        }

        for (int i = 0; i < colliders.Count; i++)
        {
            if (colliders[i].gameObject != this.gameObject && colliders[i].gameObject.tag!="Player")
            {
                if (colliders[i].GetComponent<Attachable>() != null)
                {
                    if (colliders[i].GetComponent<Attachable>().allFather != GetComponent<Attachable>().allFather)
                        EndFalling();
                }
                else if (colliders[i].tag != "Dangerous")
                    EndFalling();
            }
        }
    }

    private void EndFalling()
    {
        rb2D.velocity = new Vector2(0, 0);
        if(rb2D.GetComponent<FallingBlock>().curPlayer !=null)
        {
            rb2D.GetComponent<FallingBlock>().curPlayer.GetComponent<Rigidbody2D>().isKinematic = false;
            rb2D.GetComponent<FallingBlock>().curPlayer.transform.parent = null;
        }
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag=="Player" && Mathf.Abs(collision.collider.transform.position.x-transform.position.x)<0.9f)
        {
            GameObject player = collision.collider.gameObject;
            if(player.transform.position.y<transform.position.y-0.7f && player.GetComponent<PlayerMovement>().IsGrounded && isFalling)
            {
                player.GetComponent<Player>().GetDamage();
            }
        }
        else if(collision.collider.tag != "Player")
        {
            Debug.Log("OUCH!");
            if(collision.collider.transform.position.y<transform.position.y-0.2f && rb2D.velocity.y<0)
            {
                rb2D.velocity = new Vector2(0, 0);
                /*
                if (curPlayer != null)
                {
                    curPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    curPlayer.GetComponent<Rigidbody2D>().isKinematic = false;
                }
                */
            }
        }
    }
}
