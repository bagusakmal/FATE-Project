using System.Collections;
using TMPro;
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

    private void Start()
    {
        StartCoroutine(TypeOpeningText());
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
