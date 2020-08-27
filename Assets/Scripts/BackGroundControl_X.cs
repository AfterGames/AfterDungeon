using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundControl_X : MonoBehaviour
{
    public Vector3 playerOriginalPos;
    public Vector3 originalPos;
    public float ratio;
    private void Awake()
    {
        playerOriginalPos = Player.instance.transform.position;
        originalPos = transform.position;
    }
    private void Update()
    {
        transform.position = originalPos + Vector3.right * (Player.instance.transform.position.x - playerOriginalPos.x) * ratio;
    }
}
