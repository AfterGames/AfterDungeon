using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataWielder : MonoBehaviour
{
    public static SaveDataWielder instance;
    public Vector2 spawnPoint;

    public List<int> collectedStars = new List<int>();

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
