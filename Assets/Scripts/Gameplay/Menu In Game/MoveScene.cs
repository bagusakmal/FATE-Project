using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MoveScene : MonoBehaviour
{
    [SerializeField] private int requiredItemIndex;
    [SerializeField] private ListItem itemList;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private GameObject hiddenObject;
    [SerializeField] private GameObject hiddenObject2;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private AudioClip transitionSound;
    [SerializeField] private Animator hiddenObject3Animator; // Animator for hiddenObject3
    [SerializeField] private float delayBeforeTransition = 2.0f;

    private bool canProceed = false;
    private bool isTransitioning = false;

    private void Update()
    {
        if (canProceed && Input.GetKeyDown(KeyCode.F) && !isTransitioning)
        {
            isTransitioning = true;
            LoadSceneWithLoadingScreen();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CheckAndEnableObject();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canProceed = false;

            if (hiddenObject != null)
            {
                hiddenObject.SetActive(false);
            }
            if (hiddenObject2 != null)
            {
                hiddenObject2.SetActive(false);
            }
        }
    }

    private void CheckAndEnableObject()
    {
        if (itemList != null && itemList.HasItemBeenCollected(requiredItemIndex))
        {
            if (hiddenObject != null)
            {
                hiddenObject.SetActive(true);
            }
            canProceed = true;
        }
        else
        {
            if (hiddenObject2 != null)
            {
                hiddenObject2.SetActive(true);
            }
            canProceed = false;
        }
    }

    private void LoadSceneWithLoadingScreen()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            StartCoroutine(TransitionWithSound());
        }
        else
        {
            Debug.LogError("Scene to load is not specified in the Inspector.");
        }
    }

    private IEnumerator TransitionWithSound()
    {
        if (transitionSound != null)
        {
            // Play transition sound
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            audioSource.clip = transitionSound;
            audioSource.Play();

            // Trigger animation instead of directly activating the GameObject
            if (hiddenObject3Animator != null)
            {
                hiddenObject3Animator.SetTrigger("TransitionTrigger");
            }
        }

        // Continue with the loading screen transition
        yield return new WaitForSeconds(delayBeforeTransition);

        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        float timer = 0f;

        loadingScreen.SetActive(true);
        loadingSlider.value = 0;
        loadingText.text = "Loading: 0%";

        while (timer < 2.0f)
        {
            float progress = Mathf.Clamp01(timer / 2.0f);
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

        // Reset isTransitioning after the scene transition is complete
        isTransitioning = false;
    }
}
