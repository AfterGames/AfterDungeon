using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterGateReset : ResetableObject
{
    private ClusterButtonGate cbg;
    private void Awake()
    {
        cbg = GetComponent<ClusterButtonGate>();
    }

    public override void Reset()
    {
        cbg.Close();
    }
}
