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
        public float cameraSize;
    }
    public bool skippable;
    public SpeechBubbleCtrl SpeechBubblePrefab;
    private SpeechBubbleCtrl speechBubble;
    private int currentIndex = -1;

    public List<SpeechBubble> dialogue;
    [Tooltip("대화에 참여하는 인물들의 BubblePoint를 끌어다 놓으면 됨.")]
    public List<GameObject> SpeakingObjects;
    public List<Animator> Animators;

    //주인공을 0번에 놓고, 나머지 인물들을 차례대로 놓아서 번호를 할당받게 하는 리스트

    private Player player;
    //private Camera mainCamera;
    private Transform canvas;

    [SerializeField]
    private bool isStarted;
    //private bool keyPressed;

    private Canvas c;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        c = FindObjectOfType<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceCamera;
        c.worldCamera = Camera.main;
        canvas = c.transform;
        c.renderMode = RenderMode.ScreenSpaceOverlay;
    }

    private void Start()
    {
        c.sortingLayerName = "UI";
    }

    void Update()
    {
        if (isStarted)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)||Input.GetKeyDown(KeyCode.Z)||Input.GetKeyDown(KeyCode.Return))
                if (!speechBubble.Talking || skippable)
                    NextTalk();
        }
    }


    public void StartTalk()
    {
        player.specialControl = true;
        isStarted = true;
        CameraController.instance.talking = true;
        if(speechBubble == null)
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
            currentBubble = dialogue[currentIndex];
            if (currentIndex > 0)
            {
                int previousTalker = dialogue[currentIndex - 1].personNum;
                int currentTalker = dialogue[currentIndex].personNum;
                if (Animators[previousTalker] != null && previousTalker != currentTalker)
                {
                    Animators[previousTalker].SetTrigger("StopTalking");
                }
            }
            if (Animators[currentBubble.personNum] != null)
            {
                Animators[currentBubble.personNum].SetTrigger("TalkingMotion");
            }
            speechBubble.gameObject.SetActive(false);
            Camera mc = Camera.main;
            if (currentBubble.content == "") NextTalk();
            if (currentBubble.cameratarget == null)
                SetBubble();
            else
            {
                CameraController.instance.d = this;
                float cs = currentBubble.cameraSize;
                CameraController.instance.MoveAndScale(currentBubble.cameratarget.transform.position + Vector3.back * 10, cs, cs != 0);
            }


        }
    }

    public void SetBubble()
    {
        speechBubble.gameObject.SetActive(true);
        speechBubble.SetTail(currentBubble.tailLocation);
        speechBubble.SetLocation(SpeakingObjects[currentBubble.personNum].transform.position, ref c);
        speechBubble.SetText(currentBubble.content);
    }

    public void EndTalk()
    {
        StartCoroutine(DelayedPlayerCanControl());
        isStarted = false;
        CameraController.instance.talking = false;
        Destroy(GetComponent<BoxCollider2D>());
        speechBubble.gameObject.SetActive(false);
    }
    IEnumerator DelayedPlayerCanControl()
    {
        yield return new WaitForSeconds(0.2f);
        player.CanControl(true);
        player.specialControl = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().StopMoving();
            Debug.Log("대화 트리거");
            StartCoroutine(DelayedTalk());
        }
    }

    IEnumerator DelayedTalk()
    {
        yield return new WaitForSeconds(0.5f);
        StartTalk();
    }
}

public enum TailLoc
{
    Left = -1, Middle, Right
}
