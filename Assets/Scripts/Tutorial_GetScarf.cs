using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Tutorial_GetScarf : MonoBehaviour
{
    private bool started = false;
    public PlayableDirector fall;
    public TimelineAsset tla;
    //public GameObject button;
    // Start is called before the first frame update
    void Start()
    {
        //button.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!started && collision.tag=="Player")
        {
            started = true;
            //GetComponent<SpriteRenderer>().enabled = false;
            Player player = collision.gameObject.GetComponent<Player>();
            player.StopMoving();
            player.fireLock = true;
            //collision.gameObject.GetComponent<PlayerMovement>().GetScarf();
            //fall.enabled = false;
            //button.SetActive(true);
            Destroy(GetComponent<BoxCollider2D>());
            //gameObject.SetActive(false);
            fall.Play(tla);
        }
    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (!started && collision.tag == "Player")
        {
            started = true;
            GetComponent<SpriteRenderer>().enabled = false;
            collision.gameObject.GetComponent<Player>().fireLock = false;
            collision.gameObject.GetComponent<Player>().specialControl = false;
            collision.gameObject.GetComponent<PlayerMovement>().GetScarf();
            fall.enabled = false;
            button.SetActive(true);
            gameObject.SetActive(false);
        }
    }*/

}
