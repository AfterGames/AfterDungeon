using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThankYou : MonoBehaviour
{
    public bool demo;
    public void Thankyou()
    {
        StartCoroutine(aaa());
    }

    IEnumerator aaa()
    {

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(demo ? "Main_Demo" : "Main");
    }
}
