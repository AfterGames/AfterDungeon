﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Saver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static void LoadData(int slotNum)
    {
        if(PlayerPrefs.GetInt("saveType_" + slotNum.ToString())==(int)SaveType.Stage)
        {
            DataAdmin.instance.SetData(DataType.slotNum, slotNum);
            DataAdmin.instance.SetData(DataType.game_world, PlayerPrefs.GetInt("worldNum_" + slotNum.ToString()));
            DataAdmin.instance.SetData(DataType.game_stage, PlayerPrefs.GetInt("stageNum_" + slotNum.ToString()));
            DataAdmin.instance.SetData(DataType.deathNum, PlayerPrefs.GetInt("deathNum_" + slotNum.ToString()));
            SceneManager.LoadScene(PlayerPrefs.GetString("sceneName_" + slotNum.ToString()));
        }
        else
        {

        }
    }

    public static void SaveData(int slotNum)
    {
        PlayerPrefs.SetInt("stageNum_" + slotNum.ToString(), FindObjectOfType<Player>().stageNum);
        Debug.Log(FindObjectOfType<Player>().stageNum);
        Debug.Log("slot: " + slotNum);
        PlayerPrefs.SetInt("worldNum_" + slotNum.ToString(), DataAdmin.instance.GetWorldNum(SceneManager.GetActiveScene().name));
        Debug.Log(DataAdmin.instance.GetWorldNum(SceneManager.GetActiveScene().name));
        PlayerPrefs.SetInt("deathNum_" + slotNum.ToString(), DataAdmin.instance.GetData(DataType.deathNum));
        PlayerPrefs.SetInt("saveType_" + slotNum.ToString(), (int)SaveType.Stage);
        PlayerPrefs.SetString("sceneName_" + slotNum.ToString(), SceneManager.GetActiveScene().name);

        PlayerPrefs.Save();
    }

    public static void SaveDataOnMap(int slotNum)
    {

    }

    public static void BICDemoSave(Vector2 spawnPoint, List<int> collected)
    {
        PlayerPrefs.SetFloat("regionX", spawnPoint.x);
        PlayerPrefs.SetFloat("regionY", spawnPoint.y);
        string collectedStars = "";
        if(collected.Count > 0)
        for(int i = 0; i < collected.Count; i++)
        {
            collectedStars += (collected[i].ToString() + " ");
        }
        PlayerPrefs.SetString("collected", collectedStars);
        PlayerPrefs.Save();
    }

    public static void BICDemoLoad()
    {
        var inst = SaveDataWielder.instance;
        if (PlayerPrefs.HasKey("regionX"))
        {
            inst.spawnPoint =  new Vector2(PlayerPrefs.GetFloat("regionX"), PlayerPrefs.GetFloat("regionY"));

            string[] numberStrings = PlayerPrefs.GetString("collected").Split(' ');
            if(numberStrings.Length > 0)
                for(int j = 0; j < numberStrings.Length - 1; j++)
                {
                    Debug.Log(numberStrings[j]);
                    inst.collectedStars.Add(int.Parse(numberStrings[j]));
                }
        }
        else
        {
            Destroy(inst.gameObject);
        }
    }
}
