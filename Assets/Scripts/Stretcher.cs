using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stretcher : MonoBehaviour
{
    public StretcherManager mgrPrefab;
    public bool stretched;
    private bool stretchedAtStart;
    private Transform child;

    private AudioSource audioSource;
    public AudioClip stretchSound;
    public AudioClip shrinkSound;

    public enum Direction { up, down, left, right}
    public Direction direction;
    private void Awake()
    {
        stretchedAtStart = stretched;
        audioSource = GetComponent<AudioSource>();
        child = transform.GetChild(0);
        switch(direction)
        {
            case Direction.up:
                transform.up = Vector3.up;
                break;
            case Direction.down:
                transform.up = Vector3.down;
                break;
            case Direction.left:
                transform.up = Vector3.left;
                break;
            case Direction.right:
                transform.up = Vector3.right;
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (StretcherManager.instance == null)
            StretcherManager.instance = Instantiate(mgrPrefab);
        if (!StretcherManager.instance.stretchers.Contains(this))
            StretcherManager.instance.stretchers.Add(this);
    }

    public void Move(float x)
    {
        if(stretched)
        {
            x = 1f - 0.667f * x;
            child.localScale = new Vector3(1, x, 1);
        }
        else
        {
            x = 0.333f + 0.667f * x;
            child.localScale = new Vector3(1, x, 1);
        }
        child.localPosition = new Vector3(0, 4 * x - 2, 0);
    }

    public void Reset()
    {
        stretched = stretchedAtStart;
    }

    public void Play()
    {
        if (shrinkSound == null || stretchSound == null) return;
        audioSource.clip = stretched ? shrinkSound : stretchSound;
        audioSource.Play();
    }
}
