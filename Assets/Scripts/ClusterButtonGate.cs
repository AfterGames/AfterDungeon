using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterButtonGate : MonoBehaviour
{
    public List<ClusterButton> buttons = new List<ClusterButton>();
    //List<GameObject> platforms = new List<GameObject>();

    private void Awake()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gate = this;
        }

    }

    public void Check()
    {
        Debug.Log("check");
        for (int i = 0; i < buttons.Count; i++)
        {
            if (!buttons[i].turnedOn) return;
        }
        Open();
    }

    private void Open()
    {
        Debug.Log("open");
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }
}
