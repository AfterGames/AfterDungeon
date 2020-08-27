using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_PlayerFollower : MonoBehaviour
{
    public GameObject mid;
    public float ratio;

    void Update()
    {
        var v = transform.position;
        v.x = Player.instance.transform.position.x + 6.2f;
        var transition = v - transform.position;
        mid.transform.localPosition -= transition * ratio;
        transform.position = v;
    }
}
