using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretcherManager : MonoBehaviour
{
    public static StretcherManager instance;
    public List<Stretcher> stretchers = new List<Stretcher>();

    public float moveTime;
    public float pauseTime;

    enum State { moving, pause }
    State currentState = State.pause;

    private float elapsedTime = 0;

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (currentState == State.moving)
        {
            if (elapsedTime >= moveTime)
            {
                elapsedTime = 0;
                for (int i = 0; i < stretchers.Count; i++)
                {
                    stretchers[i].Move(1);
                }
                currentState = State.pause;
            }
            else
            {
                for(int i = 0; i < stretchers.Count; i++)
                {
                    stretchers[i].Move(elapsedTime / moveTime);
                }
            }
        }
        else
        {
            if(elapsedTime >= pauseTime)
            {
                elapsedTime = 0;
                currentState = State.moving;
                for (int i = 0; i < stretchers.Count; i++)
                {
                    stretchers[i].Play();
                    stretchers[i].stretched = !stretchers[i].stretched;
                }
            }
        }
    }

    public void Reset()
    {
        currentState = State.pause;
        elapsedTime = 0;
    }
}
