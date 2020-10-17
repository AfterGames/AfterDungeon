using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameMenu : UICluster
{
    private bool Esc;

    protected override void Start()
    {
        ActivateAll(isOn);
    }

    protected override void Update()
    {
        base.Update();
    }


    public void SetTimeScale(float time)
    {
        Time.timeScale = time;
    }

    public void Restart()
    {
        FindObjectOfType<Player>().GetFalseDamage();
    }

    public void SaveAndExit()
    {
        if(SceneManager.GetActiveScene().name == "BIC_Demo")
        {
            if(SpawnController.instance.CurRegion != SpawnController.instance.startRegion)
            {
                Saver.BICDemoSave(SpawnController.instance.CurRegion.transform.position, CollectableManager.instance.collectedStars);
            }
        }
        else
        {
            Saver.SaveData(DataAdmin.instance.GetData(DataType.slotNum));
        }
        StartCoroutine(DelayedSceneChange());
    }
    IEnumerator DelayedSceneChange()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Main_Demo");

    }

    public void ChapterRestart()
    {
        Player player = FindObjectOfType<Player>();
        player.SetSpawnPos(FindObjectOfType<SpawnController>().transform.GetChild(0).GetComponent<SpawnRegion>().spawnPositionObject.transform.position, 0, 0, 0);
        player.GetFalseDamage();
    }
}
