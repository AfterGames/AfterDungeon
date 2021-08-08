using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarfOnWood : MonoBehaviour
{
    private Transform playerT;
    private bool scarfFell = false;
    [SerializeField] private GameObject fallingScarf;
    [SerializeField] private GameObject scarfLight;

    [SerializeField] private float minDistance;

    private void Awake()
    {
        playerT = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        if (!scarfFell && Vector3.Distance(playerT.position, transform.position) <= minDistance)
        {
            scarfFell = true;
            GetComponent<SpriteRenderer>().enabled = false;
            fallingScarf.SetActive(true);

            GetComponent<Animator>().SetTrigger("Trigger");
        }
    }

    public void OnScarfFall()
    {
        scarfLight.SetActive(false);
        FindObjectOfType<CutSceneController>().StartCutScene(1);
        
        FindObjectOfType<Player>().canControlSetter = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, minDistance);
    }
}
