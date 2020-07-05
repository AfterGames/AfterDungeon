using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [System.Serializable]
    public class SpeechBubble
    {
        [Tooltip("'SpeakingObjects'에서 말하는 인물 번호")]
        public int personNum;
        [Tooltip("말풍선 꼬리의 위치")]
        public TailLoc tailLocation;
        [Tooltip("말풍선에 들어갈 말")]
        public string content;
        [Tooltip("카메라가 따라갈 게임오브젝트. 따라갈 오브젝트가 딱히 없다면 비워둬야함")]
        public GameObject cameratarget;
        [Tooltip("카메라 크기")]
        public int cameraSize;
    }

    public static Dialogue instance;

    public SpeechBubbleCtrl SpeechBubblePrefab;
    private SpeechBubbleCtrl speechBubble;
    private int currentIndex = -1;

    public List<SpeechBubble> dialogue;
    [Tooltip("대화에 참여하는 인물들의 BubblePoint를 끌어다 놓으면 됨.")]
    public List<GameObject> SpeakingObjects;

    //주인공을 0번에 놓고, 나머지 인물들을 차례대로 놓아서 번호를 할당받게 하는 리스트

    private Player player;
    //private Camera mainCamera;
    public Transform canvas;

    [SerializeField]
    private bool isStarted;
    //private bool keyPressed;


    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Canvas c = FindObjectOfType<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceCamera;
        c.worldCamera = Camera.main;
        canvas = c.transform;
    }


    void Update()
    {
        if (isStarted)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) NextTalk();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1)) StartTalk();
    }

    public void StartTalk()
    {
        player.specialControl = true;
        isStarted = true;
        CameraController.instance.talking = true;
        speechBubble = Instantiate(SpeechBubblePrefab, canvas);
        NextTalk();
    }

    private SpeechBubble currentBubble;
    public void NextTalk()
    {
        if (++currentIndex >= dialogue.Count)
        {
            EndTalk();
        }
        else
        {
            speechBubble.gameObject.SetActive(false);
            Camera mc = Camera.main;
            currentBubble = dialogue[currentIndex];
            if (currentBubble.cameratarget == null)
                SetBubble();
            else
                StartCoroutine(CameraController.instance.MoveAndScale(currentBubble.cameratarget.transform.position + Vector3.back * 10, currentBubble.cameraSize, true));
        }
    }

    public void SetBubble()
    {
        speechBubble.gameObject.SetActive(true);
        speechBubble.SetTail(currentBubble.tailLocation);
        speechBubble.SetText(currentBubble.content);
        Vector3 pos = SpeakingObjects[currentBubble.personNum].transform.position;
        Debug.Log(pos);
        speechBubble.transform.position = pos;
        Vector3 lp = speechBubble.transform.localPosition;
        speechBubble.transform.localPosition = new Vector3(lp.x, lp.y, 0);
    }

    public void EndTalk()
    {
        player.specialControl = false;
        isStarted = false;
        CameraController.instance.talking = false;
        Destroy(speechBubble.gameObject);
    }
}

public enum TailLoc
{
    Left = -1, Middle, Right
}
