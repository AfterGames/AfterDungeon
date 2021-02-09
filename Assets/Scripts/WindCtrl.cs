using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCtrl : MonoBehaviour
{
    public enum Direction { Right = 1, Left = -1 }
    public Direction direction;
    public float blowTime;
    public float stopTime;
    public float speed;
    private bool blowing;
    private float elapsedTime;

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(blowing)
        {
            if(elapsedTime >= blowTime)
            {
                PlayerMovement_Kinematic.instance.windVelocity = Vector2.zero;
                elapsedTime = 0;
                blowing = false;
            }
        }
        else
        {
            if(elapsedTime >= stopTime)
            {
                PlayerMovement_Kinematic.instance.windVelocity = new Vector2((direction == Direction.Right ? 1 : -1) * speed, 0);
                elapsedTime = 0;
                blowing = true;
            }
        }
    }

    public void Reset()
    {
        elapsedTime = 0;
        blowing = false;
        PlayerMovement_Kinematic.instance.windVelocity = Vector2.zero;
    }
}
