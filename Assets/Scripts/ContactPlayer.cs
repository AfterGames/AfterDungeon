using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContactPlayer : MonoBehaviour
{
    public abstract void OnPlayerEnter(GameObject player);

    public abstract void OnPlayerStay(GameObject player);

    public abstract void OnPlayerExit(GameObject player);

    public abstract void OnWallEnter(GameObject player);

}
