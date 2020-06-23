using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpike : Dangerous
{
    public Transform Up;
    public Transform Down;
    public Transform Left;
    public Transform Right;

    public FourDirection direction;

    private Vector2 checkBox = new Vector2(0.5f, 0.5f);

    void Start()
    {
        if (direction == FourDirection.up)
        {
            CheckSidePlatform(Up.position);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
        else if (direction == FourDirection.down)
        {
            CheckSidePlatform(Down.position);
        }
        else if (direction == FourDirection.left)
        {
            CheckSidePlatform(Left.position);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
        }
        else
        {
            CheckSidePlatform(Right.position);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }
    }

    void CheckSidePlatform(Vector2 checkPosition)
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Collider2D[] colls = Physics2D.OverlapBoxAll(checkPosition, checkBox, 0);
        foreach (Collider2D coll in colls)
        {
            colliders.Add(coll);
        }

        for (int i = 0; i < colliders.Count; i++)
        {
            if (colliders[i].tag == "Platform")
            {
                transform.parent = colliders[i].transform;
                break;
            }
        }

    }

    public enum FourDirection
    {
        up,down,left,right
    }


}
