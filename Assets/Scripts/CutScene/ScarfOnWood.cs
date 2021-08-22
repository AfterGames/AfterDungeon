using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarfOnWood : MonoBehaviour
{
    private Transform playerT;
    private bool scarfFell = false;
    [SerializeField] private GameObject fallingScarf;
    [SerializeField] private GameObject scarfLight;

    private void Awake()
    {
        playerT = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        if (!scarfFell && playerT.position.x > transform.position.x)
        {
            scarfFell = true;
            GetComponent<SpriteRenderer>().enabled = false;
            fallingScarf.SetActive(true);

            GetComponent<Animator>().SetTrigger("Trigger");
        
            FindObjectOfType<Player>().StopMoving();
        }
    }

    public void OnScarfFall()
    {
        scarfLight.SetActive(false);
        FindObjectOfType<CutSceneController>().StartCutScene(1);
    }
}
