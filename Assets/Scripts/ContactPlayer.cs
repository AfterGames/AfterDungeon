using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContactPlayer : MonoBehaviour
{
    public Vector2 currentVelocity;

    public abstract void OnPlayerEnter(PlayerMovement_parent player);

    public abstract void OnPlayerStay(PlayerMovement_parent player);

    public abstract void OnPlayerExit(PlayerMovement_parent player);

    public abstract void OnWallEnter(PlayerMovement_parent player);

}
