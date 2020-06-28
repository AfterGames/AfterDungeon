using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [System.Serializable]
    public class SpeeechBubble
    {
        [Tooltip("'SpeakingObjects'에서 말하는 인물 번호")]
        public int personNum;
        [Tooltip("말풍선 꼬리의 위치")]
        public TailLoc tailLocation;
        [Tooltip("말풍선에 들어갈 말")]
        public Text text;
        [Tooltip("카메라가 따라갈 게임오브젝트. 따라갈 오브젝트가 딱히 없다면 비워둬야함")]
        public GameObject cameratarget;
        [Tooltip("카메라 크기")]
        public int cameraSize;
    }

    public GameObject SpeechBubblePrefab;

    public List<SpeeechBubble> dialogue;
    [Tooltip("대화에 참여하는 인물들을 끌어다 놓으면 됨.")]
    public List<GameObject> SpeackingObjects;

    //주인공을 0번에 놓고, 나머지 인물들을 차례대로 놓아서 번호를 할당받게 하는 리스트

    private GameObject player;
    private GameObject mainCamera;

    [SerializeField]
    private bool isStarted;
    private bool keyPressed;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }


    void Update()
    {
        if(isStarted)
        {

        }
    }

    public void StartTalk()
    {
        player.GetComponent<Player>().specialControl = true;
        isStarted = true;
    }

    public void NextTalk()
    {

    }

    public void EndTalk()
    {

    }
}

public enum TailLoc
{
    Left = -1, Middle, Right
}
