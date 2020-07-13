using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCluster : UICluster
{
    [SerializeField] UICluster saveCluster;

    public void GoNextCluster()
    {
        //Debug.Log(PlayerPrefs.HasKey("worldNum_1"));
        //Debug.Log(PlayerPrefs.GetInt("worldNum_1"));
        ActivateAll(false);
        saveCluster.ActivateAll(true);
        Debug.Log("nextCluster");
    }
}
