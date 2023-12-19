using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueEnding : MonoBehaviour
{
    public GameObject objectToShow;

    [System.Serializable]
    public class DialogLine
    {
        public string characterName;
        public string dialogText;
        public Sprite backgroundSprite;
    }

    [System.Serializable]
    public class ScrollingText
    {
        public string text;
        public float scrollSpeed;
    }

    public TMP_Text nameText;
    public TMP_Text dialogText;
    public List<DialogLine> dialogLines;
    public float textSpeed = 0.05f;
    public Image characterImage;
    public Image backgroundImage;
    public Sprite[] characterSprites;
    public Image fadeImageToShow;
    public Image fadeImageToShow2;
    public TMP_Text textHideImage;
    public TMP_Text textHideImage2;
    public float fadeDuration = 1.0f;
    public float delayBeforeFading = 2.0f;
    private bool allowInput = false;
    private PlayerControl playerControl;
    private PlayerCombatController playerCombatController;
    public List<ScrollingText> scrollingTextList; // New: List of scrolling texts

    private int currentLine = 0;
    private bool isTyping = false;
    private bool cancelTyping = false;

    private void Start()
    {
        StartCoroutine(StartDialog());
        playerControl = FindObjectOfType<PlayerControl>();
        playerCombatController = FindObjectOfType<PlayerCombatController>();
        playerControl.enabled = false;
        playerCombatController.enabled = false;
    }

    private void Update()
    {
        if (allowInput && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                cancelTyping = true;
            }
            else if (currentLine < dialogLines.Count)
            {
                ShowNextLine();
            }
            else
            {
                CloseDialog();
            }
        }
    }

    IEnumerator StartDialog()
    {
        allowInput = false;
        yield return new WaitForSeconds(1f);
        allowInput = true;
        ShowNextLine();
    }

    void ShowNextLine()
    {
        if (currentLine < dialogLines.Count)
        {
            DialogLine currentDialog = dialogLines[currentLine];
            nameText.text = currentDialog.characterName;
            ChangeCharacterImage(currentLine);

            if (currentLine == 33)
            {
                StartCoroutine(ShowHideImageAndContinue());
            }
            if (currentLine == 94)
            {
                StartCoroutine(ShowHideImageAndContinue2());
            }
            else
            {
                ChangeBackgroundImage(currentDialog.backgroundSprite);
                StartCoroutine(TypeText(currentDialog.dialogText));
                currentLine++;
            }
        }
        else
        {
            CloseDialog();
        }
    }

    IEnumerator ShowHideImageAndContinue()
{
    allowInput = false;

    // Show the image
    fadeImageToShow.gameObject.SetActive(true);

    yield return new WaitForSeconds(delayBeforeFading);

    // Show scrolling text
    foreach (ScrollingText scrollingText in scrollingTextList)
    {
        StartCoroutine(ScrollText(scrollingText.text, scrollingText.scrollSpeed));
        yield return new WaitForSeconds(scrollingText.text.Length * scrollingText.scrollSpeed);
    }

    // Show textHideImage during image hiding
    textHideImage.gameObject.SetActive(true);

    yield return new WaitForSeconds(1f);

    // Hide textHideImage after 1 second
    textHideImage.gameObject.SetActive(false);

    // Hide the image
    fadeImageToShow.gameObject.SetActive(false);

    yield return new WaitForSeconds(2f); // Adjust the duration as needed

    allowInput = true;

    currentLine++;
    ShowNextLine();
}
IEnumerator ShowHideImageAndContinue2()
{
    allowInput = false;

    // Show the image
    fadeImageToShow2.gameObject.SetActive(true);

    yield return new WaitForSeconds(delayBeforeFading);

    // Show scrolling text
    foreach (ScrollingText scrollingText in scrollingTextList)
    {
        StartCoroutine(ScrollText(scrollingText.text, scrollingText.scrollSpeed));
        yield return new WaitForSeconds(scrollingText.text.Length * scrollingText.scrollSpeed);
    }

    // Show textHideImage during image hiding
    textHideImage2.gameObject.SetActive(true);

    yield return new WaitForSeconds(1f);

    // Hide textHideImage after 1 second
    textHideImage2.gameObject.SetActive(false);

    // Hide the image
    fadeImageToShow2.gameObject.SetActive(false);

    yield return new WaitForSeconds(2f); // Adjust the duration as needed

    allowInput = true;

    currentLine++;
    ShowNextLine();
}

    IEnumerator ScrollText(string text, float scrollSpeed)
    {
        isTyping = true;
        cancelTyping = false;
        dialogText.text = "";

        for (int i = 0; i < text.Length; i++)
        {
            if (cancelTyping)
            {
                dialogText.text = text;
                break;
            }

            dialogText.text += text[i];
            yield return new WaitForSeconds(scrollSpeed);
        }

        isTyping = false;
    }

    IEnumerator TypeText(string line)
    {
        isTyping = true;
        cancelTyping = false;
        dialogText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            if (cancelTyping)
            {
                dialogText.text = line;
                break;
            }

            dialogText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    void ChangeCharacterImage(int lineIndex)
    {
        if (characterImage != null && characterSprites.Length > 0)
        {
            int spriteIndex = lineIndex % characterSprites.Length;
            characterImage.sprite = characterSprites[spriteIndex];
        }
    }

    void ChangeBackgroundImage(Sprite backgroundSprite)
    {
        if (backgroundImage != null && backgroundSprite != null)
        {
            backgroundImage.sprite = backgroundSprite;
        }
    }

    void CloseDialog()
    {
        objectToShow.SetActive(true);
        playerControl.enabled = true;
        playerCombatController.enabled = true;
    }

    private bool IsInputAllowed()
    {
        return allowInput;
    }
}
