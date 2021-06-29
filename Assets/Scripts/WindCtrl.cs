using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindCtrl : MonoBehaviour
{
    public AudioSource source;
    [SerializeField]
    int region;
    public enum Direction { Right = 1, Left = -1 }
    public Direction direction;
    public float blowTime;
    public float stopTime;
    public float speed;
    private bool blowing;
    private float elapsedTime;

    public Image windArrow;

    private void Start()
    {
        region = CameraController.instance.WhichRegion(transform.position.x, transform.position.y);
        windArrow = GameObject.Find("windArrow").GetComponent<Image>();
    }

    private bool vfxFadeOut = false;
    private void Update()
    {
        if (region != CameraController.instance.regionNum)
        {
            if (blowing)
            {
                windArrow.color = Color.clear;
                Reset();
            }
            return;
        }
        //Debug.Log("Player at " + CameraController.instance.regionNum);
        elapsedTime += Time.deltaTime;
        if(blowing)
        {
            if(elapsedTime >= blowTime)
            {
                PlayerMovement_Kinematic.instance.windVelocity = Vector2.zero;
                elapsedTime = 0;
                blowing = false;
                source.Stop();
                StartCoroutine(WindFade(false));

            }
        }
        else
        {
            if(elapsedTime >= stopTime)
            {
                vfxFadeOut = false;
                PlayerMovement_Kinematic.instance.windVelocity = new Vector2((direction == Direction.Right ? 1 : -1) * speed, 0);
                elapsedTime = 0;
                blowing = true;
                source.Play();
            }

            else if(elapsedTime >= stopTime - 0.5f)
            {
                if(!vfxFadeOut)
                {
                    vfxFadeOut = true;
                    StartCoroutine(WindFade(true));
                }
                
            }
            //else
            //{
            //    windArrow.color = Color.clear;
            //}
        }
    }
    
    private IEnumerator WindFade(bool fadeIn)
    {
        windArrow.transform.rotation = direction == Direction.Left ? Quaternion.Euler(0,0,180) : Quaternion.identity;
        windArrow.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        windArrow.color = new Color(1, 1, 1, fadeIn ? 1 : 0);
    }

    public void StopWindByRegionChange()
    {

    }

    public void Reset()
    {
        elapsedTime = 0;
        blowing = false;
        PlayerMovement_Kinematic.instance.windVelocity = Vector2.zero;
        source.Stop();
    }
}
