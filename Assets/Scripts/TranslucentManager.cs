using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TranslucentManager : MonoBehaviour
{
    public static TranslucentManager instance;

    public static Tilemap tm;

    public List<Vector2> translucentPositions = new List<Vector2>();

    private void Awake()
    {
        instance = this;
        tm = FindObjectOfType<Tilemap>();
    }
}
