using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningText : MonoBehaviour
{
    public TMP_Text textDisplay;
    public string[] sentences;
    public float typingSpeed = 0.05f;
    public float delayBetweenSentences = 1f;
    public CanvasGroup canvasGroupToHide;
    public CanvasGroup canvasGroupToShowNext;
    public GameObject UI;
    public GameObject DialogueBox;
    private PlayerControl playerControl;
    private PlayerCombatController playerCombatController;

    private void Start()
    {
        StartCoroutine(TypeOpeningText());
        playerControl = FindObjectOfType<PlayerControl>();
        playerCombatController = FindObjectOfType<PlayerCombatController>();
        playerControl.enabled = false;
        playerCombatController.enabled = false;
        DialogueBox.SetActive(false);
        UI.SetActive(false);
    }

    IEnumerator TypeOpeningText()
    {
        foreach (string sentence in sentences)
        {
            yield return TypeSentence(sentence);
            yield return new WaitForSeconds(delayBetweenSentences);
            textDisplay.text = ""; // Reset text after finishing one sentence
        }

        // Move to the next canvas or perform other actions after the text is finished
        FadeOutCanvasGroup();
        yield return new WaitForSeconds(1f); // Add a delay before moving to the next canvas
        ShowNextCanvasGroup();
    }

    IEnumerator TypeSentence(string sentence)
    {
        foreach (char letter in sentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void FadeOutCanvasGroup()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroupToHide, 0f, 1f));
    }

    void ShowNextCanvasGroup()
    {
        playerControl.enabled = true;
        playerCombatController.enabled = true;
        DialogueBox.SetActive(true);
        UI.SetActive(true);
        StartCoroutine(FadeCanvasGroup(canvasGroupToShowNext, 1f, 1f));
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, (Time.time - startTime) / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        canvasGroup.gameObject.SetActive(targetAlpha > 0); // Hide the canvas when alpha is 0
    }
}
