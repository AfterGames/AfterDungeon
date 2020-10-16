using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashGem : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spr;
    public Sprite activated;
    public Sprite deActivated;

    public AudioSource source;
    public AudioClip regen;
    public AudioClip get;

    bool isActivated;

    float elapsedTime;
    // Start is called before the first frame update

    void Start()
    {
        animator = transform.GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        isActivated = true;
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivated == false)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime>3f)
            {
                elapsedTime = 0;
                isActivated = true;
                spr.sprite = activated;
                animator.enabled = true;
                if(spr.isVisible)
                {
                    source.clip = regen;
                    source.Play();
                }

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActivated)
        {
            source.clip = get;
            
            if (collision.tag == "Player")
            {
                source.Play();
                Debug.Log("player used");
                collision.GetComponent<PlayerMovement_parent>().DashRefill();
                isActivated = false;
                spr.sprite = deActivated;
                animator.enabled = false;
            }
        }
    }

    public void EnableByReset()
    {
        isActivated = true;
        spr.sprite = activated;
    }
}
