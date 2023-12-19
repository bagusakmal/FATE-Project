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

    // Menggunakan List untuk menyimpan objek yang dihancurkan selama runtime
    private List<string> destroyedObjectsList = new List<string>();

    private void Start()
    {
        // PC = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity).GetComponent<PlayerControl>();
        // PCC = PC.GetComponent<PlayerCombatController>();

        // Load data objek yang dihancurkan dari PlayerPrefs saat game dimulai
        LoadDestroyedObjects();

        // Pastikan objek yang dihancurkan tidak muncul kembali saat game dimulai ulang
        DestroyCollectedObjects();
    }

    private void LoadDestroyedObjects()
    {
        string destroyedObjectsData = PlayerPrefs.GetString(DestroyedObjectsKey, "");
        string[] destroyedObjectsArray = destroyedObjectsData.Split(',');

        foreach (string objectName in destroyedObjectsArray)
        {
            if (!string.IsNullOrEmpty(objectName))
            {
                destroyedObjectsList.Add(objectName);
                Debug.Log("Restored Destroyed Object: " + objectName);
            }
        }
    }

    // Tambahkan fungsi ini untuk memastikan objek yang dihancurkan tidak muncul kembali saat game dimulai ulang
    private void DestroyCollectedObjects()
    {
        foreach (string objectName in destroyedObjectsList)
        {
            GameObject existingObject = GameObject.Find(objectName);

            if (existingObject != null)
            {
                Destroy(existingObject);
            }
        }
    }

    public void OnObjectDestroyed(string objectName)
    {
        Debug.Log("isrun");

        // Tambahkan objectName ke daftar jika belum ada
        if (!destroyedObjectsList.Contains(objectName))
        {
            destroyedObjectsList.Add(objectName);

            // Hapus objek jika ditemukan
            GameObject destroyedObject = GameObject.Find(objectName);
            if (destroyedObject != null)
            {
                Debug.Log("Destroyed Object: " + objectName);
                Destroy(destroyedObject);
            }
        }
    }

    private void Update()
    {
        // CheckRespawn();
        // if (GameOv)
        // {
        //     PC.walkingAudioSource.Stop();
        //     PC.dashAudioSource.Stop();
        //     PCC.attackAudioSource.Stop();
        // }
    }

    public void gameOver()
    {
        GameOv = true;
        Time.timeScale = 0;
        GameOver.SetActive(true);

        // Simpan data objek yang dihancurkan ke PlayerPrefs saat game over
        SaveDestroyedObjects();
    }

    public void restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Bersihkan daftar objek yang dihancurkan setelah me-restart permainan
        destroyedObjectsList.Clear();
    }

    private void SaveDestroyedObjects()
    {
        // Gabungkan daftar objek yang dihancurkan menjadi satu string
        string destroyedObjectsData = string.Join(",", destroyedObjectsList.ToArray());

        // Simpan ke PlayerPrefs
        PlayerPrefs.SetString(DestroyedObjectsKey, destroyedObjectsData);
        PlayerPrefs.Save();
    }
}
