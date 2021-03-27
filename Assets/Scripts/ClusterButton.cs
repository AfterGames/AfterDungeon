using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClusterButton : ContactArrow
{
    [HideInInspector]
    public ClusterButtonGate gate;
    public bool turnedOn;
    private bool TurnedOn
    {
        get { return turnedOn; }
        set
        {
            turnedOn = value;
            spr.sprite = value ? onImage : offImage;
            source.clip = value ? activateClip : deactivateClip;
            source.Play();
        }
    }

    private SpriteRenderer spr;
    public Sprite onImage;
    public Sprite offImage;

    public AudioSource source;
    public AudioClip activateClip;
    public AudioClip deactivateClip;
    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    public override void OnLodgingEnterAction(GameObject arrow)
    {
        TurnedOn = true;
        gate.Check();
        arrow.GetComponent<ProjectileController>().ArrowEnd();  
    }
    public override void OnLodgingStayAction(GameObject arrow)
    {
    }
    public override void OnLodgingExitAction(GameObject arrow)
    {
    }

    private void Update()
    {
        if(TurnedOn)
        {
            if (PlayerMovement_Kinematic.instance.IsGrounded)
                TurnedOn = false;
        }
    }
}
