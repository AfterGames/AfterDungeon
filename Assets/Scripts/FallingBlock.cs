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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(1,1));

        
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

        if (rb2D!=null &&rb2D.velocity.magnitude > Mathf.Epsilon)
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
            isFalling = true;
            rb2D.velocity = new Vector2(0, -velocity);
        }
    }

    public override void OnPlayerExit(GameObject player)
    {
    }

    public override void OnPlayerStay(GameObject player)
    {
    }

    public override void OnWallEnter(GameObject player)
    {
        OnPlayerEnter(player);
    }

    void Check()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.99f, 0.99f), 0);
        foreach (Collider2D coll in colls)
        {
            colliders.Add(coll);
        }

        for (int i = 0; i < colliders.Count; i++)
        {
            if (colliders[i].gameObject != this.gameObject && colliders[i].gameObject.tag!="Player")
            {   
                if(colliders[i].GetComponent<Attachable>()!=null)
                {
                    if(colliders[i].GetComponent<Attachable>().allFather!=GetComponent<Attachable>().allFather)
                        rb2D.velocity = new Vector2(0, 0);
                }
                else if(colliders[i].tag!="Dangerous")
                    rb2D.velocity = new Vector2(0, 0);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag=="Player")
        {
            GameObject player = collision.collider.gameObject;
            if(player.transform.position.y<transform.position.y-0.7f && player.GetComponent<PlayerMovement>().IsGrounded)
            {
                player.GetComponent<Player>().GetDamage();
            }
        }
        else
        {
            Debug.Log("OUCH!");
            if(collision.collider.transform.position.y<transform.position.y-0.2f && rb2D.velocity.y<0)
            {
                rb2D.velocity = new Vector2(0, 0);
            }
        }
    }
}
