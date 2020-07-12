using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightLight_0 : MonoBehaviour
{
    public float velocity;
    public List<GameObject> Lights;
    public bool start;

    private int curLight = 0;
    private float prop = 1;
    int i;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Lights.Count);
        for(i=0;i<Lights.Count;i++)
        {
            for (int j = 0; j < Lights[i].transform.childCount; j++)
            {
                Lights[i].transform.GetChild(j).GetComponent<LineLightController>().SetProportion(prop);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (curLight < Lights.Count && start)
        {
            prop = prop - velocity / Lights[curLight].transform.localScale.x;
            if (prop < 0.01f)
            {
                prop = 0;
                for (i = 0; i < Lights[curLight].transform.childCount; i++)
                {
                    Lights[curLight].transform.GetChild(i).GetComponent<LineLightController>().SetProportion(prop);
                }
                prop = 1;
                curLight++;
                    
            }
            else
            {
                for (i = 0; i < Lights[curLight].transform.childCount; i++)
                {
                    Lights[curLight].transform.GetChild(i).GetComponent<LineLightController>().SetProportion(prop);
                }
            }
        }
    }
}
