using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Tutorial_GetScarf : MonoBehaviour
{
    private bool started = false;
    public PlayableDirector fall;
    public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        button.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!started && collision.tag=="Player")
        {
            started = true;
            GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<Player>().fireLock = false;
            collision.gameObject.GetComponent<PlayerMovement>().GetScarf();
            fall.enabled = false;
            button.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!started && collision.tag == "Player")
        {
            started = true;
            GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<Player>().fireLock = false;
            collision.gameObject.GetComponent<PlayerMovement>().GetScarf();
            fall.enabled = false;
            button.SetActive(true);
            gameObject.SetActive(false);
        }
    }

}
