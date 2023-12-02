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
    public GameObject GameOver;

    private void Start()
    {
        // CVC = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        
    }

    private void Update()
    {
        // CheckRespawn();
    }
    // public void Respawn()
    // {
    //     respawnTimeStart = Time.time;
    //     respawn = true;
    // }

    // private void CheckRespawn()
    // {
    //     if (Time.time >= respawnTimeStart + respawnTime && respawn)
    //     {
    //         RespawnPlayer();
    //         respawn = false;
    //     }
    // }
    // private void RespawnPlayer()
    // {
    //     // Destroy all previous player objects in the scene
    //     GameObject[] previousPlayers = GameObject.FindGameObjectsWithTag("Player");
    //     foreach (var previousPlayer in previousPlayers)
    //     {
    //         Destroy(previousPlayer);
    //     }

    //     // Instantiate a new player at the respawn point
    //     GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, respawnPoint.rotation);
        
    //     // Set the z position to 0 to ensure it remains at the same depth
    //     Vector3 respawnPosition = newPlayer.transform.position;
    //     respawnPosition.z = 0;
    //     newPlayer.transform.position = respawnPosition;

    //     // Set the Cinemachine Virtual Camera to follow the new player
    //     CVC.m_Follow = newPlayer.transform;
    // }

    public void gameOver () 
    {
        GameOver.SetActive(true);
    }
    public void restart () 
    {
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
}
