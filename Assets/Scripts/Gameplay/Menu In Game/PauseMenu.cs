using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject loadingScreen;
    public Slider loadingSlider;
    public TextMeshProUGUI loadingText;
    public float loadingTime = 2.0f;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        StartCoroutine(ResumeAfterFrame());
    }

    private IEnumerator ResumeAfterFrame()
    {
        yield return null;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void BackToMainMenu()
    {
        LoadSceneWithLoadingScreen("MainMenu");
    }

    public void LoadSceneWithLoadingScreen(string sceneToLoad)
    {
        // Jika game sedang di-pause, langsung resume
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }

        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    private IEnumerator LoadSceneAsync(string sceneToLoad)
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
}
