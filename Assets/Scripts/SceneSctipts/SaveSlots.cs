﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveSlots : UIEffect
{
    //public GameObject highlightBox;
    public GameObject slotImage;
    public int slotNum;
    public GameObject noDataText;

    public Text slotNumText;
    public Text WorldNumText;
    public Text deathNumText;

    public Sprite filledSprite;

    [SerializeField]private bool hasData;
    public bool HasData
    {
        get
        {
            return hasData;
        }
        set
        {
            hasData = value;
        }
    }

    private void Awake()
    {
        hasData = PlayerPrefs.HasKey("worldNum_" + slotNum.ToString());
        if (!hasData)
        {
            for (int i=0;i<transform.childCount;i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            noDataText.SetActive(true);
        }
        else
        {
            noDataText.SetActive(false);
            slotNumText.text = "Save " + slotNum.ToString();
            WorldNumText.text = "World " + PlayerPrefs.GetInt("worldNum_" + slotNum.ToString()).ToString();
            deathNumText.text = "Death: " + PlayerPrefs.GetInt("deathNum_" + slotNum.ToString()).ToString();
            GetComponent<Image>().sprite = filledSprite;
        }
    }
    public void SetSlot()
    {
        hasData = PlayerPrefs.HasKey("worldNum_" + slotNum.ToString());
        Debug.Log(PlayerPrefs.HasKey("worldNum_" + 1.ToString()));
        if (!hasData)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            noDataText.SetActive(true);
        }
        else
        {
            noDataText.SetActive(false);
            slotNumText.text = "Save " + slotNum.ToString();
            WorldNumText.text = "World " + PlayerPrefs.GetInt("worldNum_" + slotNum.ToString()).ToString();
            deathNumText.text = "Death: " + PlayerPrefs.GetInt("deathNum_" + slotNum.ToString()).ToString();
            GetComponent<Image>().sprite = filledSprite;
        }
    }

    public override void Activate()
    {
            //highlightBox.SetActive(true);
    }
    public override void DeActivate()
    {
            //highlightBox.SetActive(false);
    }
    public override void Select()
    {
        if(hasData)
            Saver.LoadData(slotNum);
        else
        {
            DataAdmin.instance.SetData(DataType.slotNum, slotNum);
            SceneManager.LoadScene("0");
        }
        //GetComponent<Button>().onClick.Invoke();
    }

    private void Update()
    {
        hasData = PlayerPrefs.HasKey("worldNum_" + slotNum.ToString());
        Debug.Log(slotNum.ToString() + ": " + hasData);
    }
}
