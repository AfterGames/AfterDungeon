using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Tutorial_Fall : MonoBehaviour
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
        if (!started && collision.tag == "Player")
        {
            started = true;
            Player player = collision.gameObject.GetComponent<Player>();
            player.StopMoving();
            Destroy(GetComponent<BoxCollider2D>());
            fall.Play(tla);
        }
    }
}
