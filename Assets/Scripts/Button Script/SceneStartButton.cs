using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SceneStartButton : MonoBehaviour
{
    public string sceneToLoad;
    public GameObject loadingScreen;
    public Slider loadingSlider;
    public TextMeshProUGUI loadingText;
    public LevelSaveManager levelSaveManager; // Reference to the LevelSaveManager script
    public GameObject warning;

    public float loadingTime = 2.0f;

    public void LoadSceneWithLoadingScreen()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        float timer = 0f;

        loadingScreen.SetActive(true);
        loadingSlider.value = 0;
        loadingText.text = "Loading: 0%";

        while (timer < loadingTime)
        {
            float progress = Mathf.Clamp01(timer / loadingTime);
            loadingSlider.value = progress;
            loadingText.text = "Loading: " + Mathf.Round(progress * 100) + "%";

            timer += Time.deltaTime;

            yield return null;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingSlider.value = progress;
            loadingText.text = "Loading: " + Mathf.Round(progress * 100) + "%";

            yield return null;
        }
    }

    public void CheckSaveDataAndLoadScene()
    {
        // Check if there is save data in the LevelSaveManager
        if (levelSaveManager != null && levelSaveManager.HasSaveData())
        {
            // Show warning if there is save data
            if (warning != null)
            {
                warning.SetActive(true);
            }
        }
        else
        {
            // No save data, proceed to load scene
            LoadSceneWithLoadingScreen();
        }
    }
}
