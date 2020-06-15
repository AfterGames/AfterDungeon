using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Tutorial_Fall : MonoBehaviour
{
    private bool started = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
           if (!started)
           {
                collision.gameObject.GetComponent<Player>().specialControl = true;
                collision.gameObject.transform.localScale = new Vector3(1, 1, 1);
                started = true;
                StartCoroutine(Play());
            }
    }
    
    private IEnumerator Play()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<PlayableDirector>().Play();
    }
}
