using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataWielder : MonoBehaviour
{
    public static SaveDataWielder instance;
    public Vector2 spawnPoint;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
