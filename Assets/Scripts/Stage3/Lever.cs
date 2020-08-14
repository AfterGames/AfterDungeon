using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : ContactArrow
{
    public static event Action transition;
    private List<GameObject> leverPlatforms;
    private List<GameObject> leverPlatformsB;
    private bool isActive = false;
    public Animator animator;
    private bool off;

    private void Awake()
    {
        transition += Transition;
        //if (!levers.Contains(this))
        //    levers.Add(this);
        
    }

    private void Start()
    {
        leverPlatforms = new List<GameObject>();
        leverPlatformsB = new List<GameObject>();

        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Lever Platform");
        foreach (GameObject platform in platforms)
        {
            leverPlatforms.Add(platform);
        }

        GameObject[] platformsB = GameObject.FindGameObjectsWithTag("Lever PlatformB");
        foreach (GameObject platform in platformsB)
        {
            leverPlatformsB.Add(platform);
        }
        ActivatePlatform();
        if(leverPlatforms.Count == 0)
        {
            animator.SetTrigger("turnOff");
            //levers.Remove(this);
        }
    }

    public override void OnLodgingEnterAction(GameObject arrow)
    {
        if(arrow==null)
            ActivatePlatform();
        else if (!arrow.GetComponent<ProjectileController>().isEnd)
            ActivatePlatform();
        if (arrow != null)
            arrow.GetComponent<ProjectileController>().ArrowEnd();
        //arrow.GetComponent<ArrowController>().Disable();
        //animator.SetBool("On", true);
        transition();
    }

    public void Transition()
    {
        Debug.Log("lever shot");
        animator.SetTrigger("Contact");
    }

    public override void OnLodgingExitAction(GameObject arrow)
    {
    }

    public override void OnLodgingStayAction(GameObject arrow)
    {
    }

    private void ActivatePlatform()
    {

        foreach (GameObject leverPlatform in leverPlatforms)
        {
            leverPlatform.GetComponent<LeverPlatform>().ChangeState();
        }
        foreach (GameObject leverPlatform in leverPlatformsB)
        {
            leverPlatform.GetComponent<LeverPlatform>().ChangeState();
        }
    }
}
