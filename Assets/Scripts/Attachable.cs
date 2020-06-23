using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachable : MonoBehaviour
{
    public Transform Up;
    public Transform Down;
    public Transform Left;
    public Transform Right;

    private Attachable upObject;
    private Attachable downObject;
    private Attachable leftObject;
    private Attachable rightObject;

    public GameObject allFather;
    public Vector3 allFatherPosition;


    private Vector2 checkBox = new Vector2(0.5f, 0.5f);

    private void Awake()
    {
        upObject = CheckSidePlatform(Up.position);
        downObject = CheckSidePlatform(Down.position);
        leftObject = CheckSidePlatform(Left.position);
        rightObject = CheckSidePlatform(Right.position);

    }

    private void Start()
    {
        if(upObject == null && leftObject == null)
        {
            allFather = this.gameObject;
            allFatherPosition = allFather.transform.position;
            if(rightObject!=null)
                rightObject.RecursiveParenting(allFather);
            if(downObject !=null)
                downObject.RecursiveParenting(allFather);
        }
    }

    private void RecursiveParenting(GameObject Father)
    {
        if (allFather != null)
            return;

        this.transform.parent = Father.transform;
        allFather = Father;
        allFatherPosition = Father.transform.position;

        if (rightObject != null)
            rightObject.RecursiveParenting(allFather);
        if (downObject != null)
            downObject.RecursiveParenting(allFather);
        if (leftObject != null)
            leftObject.RecursiveParenting(allFather);
        if (upObject != null)
            upObject.RecursiveParenting(allFather);
    }

    Attachable CheckSidePlatform(Vector2 checkPosition)
    {
        List<Collider2D> colliders = new List<Collider2D>();
        Collider2D[] colls = Physics2D.OverlapBoxAll(checkPosition, checkBox, 0);
        foreach (Collider2D coll in colls)
        {
            colliders.Add(coll);
        }

        for (int i = 0; i < colliders.Count; i++)
        {
            if (colliders[i].GetComponent<Attachable>() != null)
            {
                return colliders[i].GetComponent<Attachable>();
            }
        }
        return null;
    }
}
