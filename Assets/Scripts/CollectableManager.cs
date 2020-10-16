using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableManager : MonoBehaviour
{
    public static CollectableManager instance;
    public int followingNum = 0;
    private int collectedNum = 0;
    public Text collectedNumberText;

    public event Action Collect;

    private void Awake()
    {
        instance = this;
        collectedNumberText = GetComponent<Text>();
    }

    public void AddCollection()
    {
        collectedNumberText.text = (++collectedNum).ToString();
        followingNum--;
    }

    public void RegionChange()
    {
        Collect.Invoke();
    }
}
