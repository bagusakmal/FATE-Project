using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPoint;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private float respawnTime;
    private float respawnTimeStart;
    private bool respawn;
    private CinemachineVirtualCamera CVC;
    private PlayerControl PC;
    private PlayerCombatController PCC;
    public GameObject GameOver;
    private bool GameOv;
    public const string DestroyedObjectsKey = "DestroyedObjects";

    private void Start()
    {
        PC = playerPrefab.GetComponent<PlayerControl>();
        PCC = playerPrefab.GetComponent<PlayerCombatController>();
        string destroyedObjectsData = PlayerPrefs.GetString(DestroyedObjectsKey, "");
        string[] destroyedObjectsArray = destroyedObjectsData.Split(',');

        foreach (string objectName in destroyedObjectsArray)
        {
            if (!string.IsNullOrEmpty(objectName))
            {
                GameObject destroyedObject = GameObject.Find(objectName);
                if (destroyedObject != null)
                {
                    Destroy(destroyedObject);
                }
            }
        }
    }

    public void OnObjectDestroyed(string objectName)
    {
        string destroyedObjectsData = PlayerPrefs.GetString(DestroyedObjectsKey, "");
        destroyedObjectsData += objectName + ",";
        PlayerPrefs.SetString(DestroyedObjectsKey, destroyedObjectsData);
        PlayerPrefs.Save();
    }


    private void Update()
    {
        // CheckRespawn();
        if (GameOv){
            PC.walkingAudioSource.Stop();
            PC.dashAudioSource.Stop();
            PCC.attackAudioSource.Stop();
        }
    }

    public void gameOver () 
    {
        
        GameOv = true;
        Time.timeScale = 0;
        GameOver.SetActive(true);
    }
    public void restart () 
    {
        Time.timeScale = 1;
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }

    
}
