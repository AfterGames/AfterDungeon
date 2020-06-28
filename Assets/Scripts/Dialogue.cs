using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [System.Serializable]
    public class SpeeechBubble
    {
        [Tooltip("말풍선 꼬리의 위치")]
        public TailLoc tailLocation;
        [Tooltip("말풍선에 들어갈 말")]
        public Text text;
        [Tooltip("카메라가 따라갈 게임오브젝트. 따라갈 오브젝트가 딱히 없다면 비워둬야함")]
        public GameObject cameratarget;
        [Tooltip("카메라 크기")]
        public int cameraSize;
    }

    public List<SpeeechBubble> dialogue;

    private GameObject player;
    private GameObject mainCamera;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }


    void Update()
    {
        
    }
}

public enum TailLoc
{
    Left = -1, Middle, Right
}
