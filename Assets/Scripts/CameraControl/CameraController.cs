using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Camera controlcamera;
    public float moveTime = 0.1f;
    float inverseMoveTime;

    private bool isCameraMoving = false;
    public CameraType startType; // 게임 시작때 카메라 정보, 사용하지 않을 수도 있음
    public int numberOfRegions;
    public Vector3 startPosition;
    private bool Talking;
    public bool talking
    {
        get { return Talking; }
        set
        {
            Talking = value;
            if (Talking)
            {
                originalPos = controlcamera.transform.position;
                originalSize = controlcamera.orthographicSize;
            }
            else
                MoveAndScale(originalPos, originalSize);
        }
    }


    public GameObject player;

    private float curLeft = 0; // 현재 영역의 각 변들
    private float curRight = 0;
    private float curUp = 0;
    private float curDown = 0;

    private float curWidth = -1; // 현재 영역의 너비, 높이
    private float curHeight = -1;

    private int regionNum;

    float x;
    float y; // player의 위치 저장용 변수

    Vector3 originalPos;
    [SerializeField]
    float originalSize;


    CameraRegion curRegion;

    private Vector3 offset; // 카메라 이동시 offset
    public SpriteRenderer backGround;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        controlcamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        originalSize = controlcamera.orthographicSize;
        regionNum = WhichRegion();
        inverseMoveTime = 1f / moveTime;
        x = player.transform.position.x;
        y = player.transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (talking)
        {
            return;
            //if(!isCameraMoving)

        }
        if (isCameraMoving == false)
        {
            //Debug.Log(offset);
            x = player.transform.position.x;
            y = player.transform.position.y;
            int num;
            //Debug.Log(curRegion.transform.position);
            //Debug.Log("Min: " + curRegion.MinPoint);
            //Debug.Log(curRegion.Min);
            // Debug.Log("Down: " + curDown);
            if ((x >= curLeft) && (x < curRight) && (y >= curDown) && (y < curUp))
            {
                 //Debug.Log("here!");
                if (curRegion.cameratype == CameraType.XFreeze)
                {
                    Vector3 next = offset + new Vector3(controlcamera.transform.position.x, y, -10f);
                    if (next.y < curRegion.Min)
                        next.y = curRegion.Min;
                    else if (next.y > curRegion.Max)
                        next.y = curRegion.Max;

                    controlcamera.transform.position = next;
                }
                else if (curRegion.cameratype == CameraType.YFreeze)
                {
                    Vector3 next = offset + new Vector3(x, controlcamera.transform.position.y, -10f);
                    if (next.x < curRegion.Min)
                        next.x = curRegion.Min;
                    else if (next.x > curRegion.Max)
                        next.x = curRegion.Max;

                    controlcamera.transform.position = next;
                }
            }
            //else if ((num = WhichRegion()) != transform.childCount)
            else if ((num = WhichRegion()) < numberOfRegions)
            {
                Debug.Log(num);
                Debug.Log("region changed");
                if (curRegion.cameratype == CameraType.Center)
                {
                    StartCoroutine(Move(curRegion.Center + new Vector3(0f, 0f, -10f)));
                }
                else if ((curRegion.MinPoint - player.transform.position).magnitude < (curRegion.MaxPoint - player.transform.position).magnitude)
                {
                    SetMinOffset();
                    StartCoroutine(Move(curRegion.MinPoint + new Vector3(0f, 0f, -10f)));
                }
                else
                {
                    SetMaxOffset();
                    StartCoroutine(Move(curRegion.MaxPoint + new Vector3(0f, 0f, -10f)));
                }
            }
            else if(y < curDown)
            {
                Player.instance.GetDamage();
            }
            else
            {
                //player.GetComponent<Player>().GetDamage();
                StartCoroutine(NextStage());
            }
        }
    }

    IEnumerator NextStage()
    {
        Player.instance.FadeOut();
        Player.instance.StopMoving();
        yield return new WaitForSeconds(0.5f);
        SceneChange();
    }
    void SceneChange() {
        if (SceneManager.GetActiveScene().name == "0")
            SceneManager.LoadScene("1");
    }

    int WhichRegion()
    {
        int i;
        for (i = 0; i < transform.childCount; i++)
        {
            CameraRegion region = transform.GetChild(i).gameObject.GetComponent<CameraRegion>();
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

    void SetMinOffset()
    {
        CameraType cameratype = curRegion.cameratype;
        if (cameratype == CameraType.XFreeze)
        {
            offset = new Vector3(0f, curRegion.Min - curDown, 0f);
        }
        else
        {
            offset = new Vector3(curRegion.Min - curLeft, 0f, 0f);
        }
    }
    void SetMaxOffset()
    {
        CameraType cameratype = curRegion.cameratype;
        if (cameratype == CameraType.XFreeze)
        {
            offset = new Vector3(0f, curUp - curRegion.Max, 0f);
        }
        else
        {
            offset = new Vector3(curRight - curRegion.Max, 0f, 0f);
        }
    }
    IEnumerator Move(Vector3 end)
    {
        isCameraMoving = true;
        float sqrRemainingDistance = (controlcamera.transform.position - end).sqrMagnitude;

        Time.timeScale = 0f;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(controlcamera.transform.position, end, inverseMoveTime * Time.fixedDeltaTime);
            Vector3 offSet = newPosition - controlcamera.transform.position;
            controlcamera.transform.position = newPosition;
            sqrRemainingDistance = (controlcamera.transform.position - end).sqrMagnitude;

            if(backGround != null && Mathf.Abs(offSet.x) > float.Epsilon)
            {
                Debug.Log(offSet);
                offSet.y = 0;
                backGround.transform.localPosition = backGround.transform.localPosition - offSet/40;
            }
            //Debug.Log(camera.transform.position);
            // Debug.Log(end);
            //Debug.Log("Move!");
            yield return null;
        }



        if (curRegion.EnterAccel)
        {
            Vector2 vel = player.GetComponent<Rigidbody2D>().velocity;
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(vel.x, vel.y + curRegion.EnterAccelMagnitude);
        }
        Time.timeScale = 1f;
        isCameraMoving = false;
    }

    private int step = 15;
    public void MoveAndScale(Vector3 endPos, float endSize, bool setBubble = false)
    {
        StartCoroutine(IMoveAndScale(endPos, endSize, setBubble));
    }

    public Dialogue d;
    private IEnumerator IMoveAndScale(Vector3 endPos, float endSize, bool setBubble = false)
    {
        Debug.Log(setBubble);
        isCameraMoving = true;
        //Time.timeScale = 0f;
        float sizeDiff = endSize - controlcamera.orthographicSize;
        float initialSize = controlcamera.orthographicSize;
        Vector3 initialPosition = controlcamera.transform.position;

        Vector3 dir = endPos - initialPosition;

        for (int i = 0; i < step; i++)
        {
            if (Vector3.Distance(controlcamera.transform.position, endPos) < float.Epsilon &&
                sizeDiff < float.Epsilon)
                break;
            controlcamera.transform.position = initialPosition + dir *  SmoothCurve(((float)i+1) / step);
            controlcamera.orthographicSize = initialSize + sizeDiff * SmoothCurve(((float)i + 1) / step);
            //Debug.Log(i);
            //Debug.Log(SmoothCurve(i + 1 / step));
            yield return new WaitForSeconds(moveTime / step);
        }

        if (setBubble && d != null)
        {
            d.SetBubble();
        }


        Time.timeScale = 1f;
        isCameraMoving = false;
    }

    private float SmoothCurve(float x)
    {
        return -2 * x * x * x + 3 * x * x;
    }
}

/*
Debug.Log("x:"+x);
Debug.Log("y:"+y);
Debug.Log("height:" + height);
Debug.Log(regionx - width);
Debug.Log(regionx + width);
Debug.Log(regiony - height);
Debug.Log(regiony + height);
*/
