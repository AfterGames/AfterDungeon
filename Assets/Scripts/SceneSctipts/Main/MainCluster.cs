using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCluster : UICluster
{
    [SerializeField] UICluster saveCluster;

    public void GoNextCluster()
    {
        ActivateAll(false);
        bool isthereData = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            isthereData = saveCluster.transform.GetChild(i).GetComponent<SaveSlots>().HasData || isthereData;
        }
        if (!isthereData)
        {
            SaveSeletion.LoadGame();
        }
        else
        {
            saveCluster.ActivateAll(true);
        }
    }
}
