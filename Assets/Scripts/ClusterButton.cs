using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClusterButton : ContactArrow
{
    public ClusterButtonGate gate;
    public bool turnedOn; 
    private bool TurnedOn
    {
        get { return turnedOn; }
        set
        {
            turnedOn = value;
            spr.sprite = value ? onImage : offImage;
        }
    }

    public SpriteRenderer spr;
    public Sprite onImage;
    public Sprite offImage;
    private void Awake()
    {
        //mover
    }

    public override void OnLodgingEnterAction(GameObject arrow)
    {
        TurnedOn = true;
        gate.Check();
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
