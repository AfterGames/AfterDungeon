using System;
using System.Linq;
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

    [HideInInspector]
    public List<int> collectedStars = new List<int>();

    private void Awake()
    {
        instance = this;
        collectedNumberText = GetComponent<Text>();
    }

    private void Start()
    {
        if(SaveDataWielder.instance != null)
        {
            collectedStars = SaveDataWielder.instance.collectedStars.ToList();
        }
    }

    public void AddCollection(bool bySaveData = false)
    {
        collectedNumberText.text = (++collectedNum).ToString();
        if(!bySaveData)
            followingNum--;
    }

    public void RegionChange()
    {
        Collect.Invoke();
    }
}
