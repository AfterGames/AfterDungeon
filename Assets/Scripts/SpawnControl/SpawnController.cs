﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour
{
    public static SpawnController instance = null;

    [Header("주의: SpawnController의 위치는 0,0,0이어야 함")]
    public SpawnRegion startRegion;

    public Player player;

    private float curLeft = 0; // 현재 영역의 각 변들
    private float curRight = 0;
    private float curUp = 0;
    private float curDown = 0;

    private float curWidth = -1; // 현재 영역의 너비, 높이
    private float curHeight = -1;

    private int regionNum;

    float x;
    float y; // player의 위치 저장용 변수


    SpawnRegion curRegion;
    public SpawnRegion CurRegion { get { return curRegion; } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        regionNum = WhichRegion();
        x = player.transform.position.x;
        y = player.transform.position.y;

        int i;
        for (i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<SpawnRegion>().IsFirst)
            {
                startRegion = transform.GetChild(i).GetComponent<SpawnRegion>();
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        x = player.transform.position.x;
        y = player.transform.position.y;

        int num;

        if ((x >= curLeft) && (x < curRight) && (y >= curDown) && (y < curUp))
        {
        }
        else if ((num = WhichRegion()) != transform.childCount)
        {
            curRegion.SetSpawn(player);
        }

    }

    int WhichRegion()// 해당하는 region 찾으면 region크기 관련 변수도 다 바꿔버린다
    {
        int i;
        for (i = 0; i < transform.childCount; i++)
        {
            SpawnRegion region = transform.GetChild(i).gameObject.GetComponent<SpawnRegion>();
            float width = region.Width;
            float height = region.Height;
            float regionx = region.gameObject.transform.position.x;
            float regiony = region.gameObject.transform.position.y;

            if ((x >= regionx - width) && (x < regionx + width) && (y >= regiony - height) && (y < regiony + height))
            {
                curWidth = region.Width;
                curHeight = region.Height;
                curLeft = regionx - width;
                curRight = regionx + width;
                curUp = regiony + height;
                curDown = regiony - height;
                regionNum = i;
                curRegion = region;
                break;
            }
        }
        return i;
    }

    public void Respawn()
    {
        Debug.Log(regionNum + " " +transform.childCount);
        if (regionNum < transform.childCount - 1)
        {
            curRegion = transform.GetChild(regionNum + 1).GetComponent<SpawnRegion>();

            regionNum++;
            Debug.Log("region num 증가 해서 "+regionNum);

            float regionx = curRegion.gameObject.transform.position.x;
            float regiony = curRegion.gameObject.transform.position.y;

            curWidth = curRegion.Width;
            curHeight = curRegion.Height;
            curLeft = regionx - curWidth;
            curRight = regionx + curWidth;
            curUp = regiony + curHeight;
            curDown = regiony - curHeight;

            curRegion.SetSpawn(player);
        }
        else
        {
            SceneChange();
        }
    }

    void SceneChange()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "0")
            SceneManager.LoadScene("1");
        else if (currentSceneName == "1")
            SceneManager.LoadScene("2");
        else
            SceneManager.LoadScene("1");
    }
}
