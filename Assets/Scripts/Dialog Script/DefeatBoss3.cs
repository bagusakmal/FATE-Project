using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DefeatBoss3 : MonoBehaviour
{
    public GameObject objectToShow;

    [System.Serializable]
    public class DialogLine
    {
        public string characterName;
        public string dialogText;
    }

    public TMP_Text nameText;
    public TMP_Text dialogText;
    public List<DialogLine> dialogLines;
    public float textSpeed = 0.05f;
    public Image characterImage;
    public Sprite[] characterSprites;

    private int currentLine = 0;
    private bool isTyping = false;
    private bool cancelTyping = false;
    public Image fadeImageToShow;  // Use the hidden image for fading in and out
    public TMP_Text textHideImage; // Text to display during image hiding
    public float fadeDuration = 1.0f;  // Set the duration of the fade
    public float delayBeforeFading = 1.0f;  // Set the delay before fading
    private bool allowInput = false;
    private PlayerControl playerControl;
    private PlayerCombatController playerCombatController;
    public GameObject UI;
    private bool isDialogueActive = false;

    private void Start()
    {
        StartCoroutine(StartDialog());
        playerControl = FindObjectOfType<PlayerControl>();
        playerCombatController = FindObjectOfType<PlayerCombatController>();
        playerControl.enabled = false;
        playerCombatController.enabled = false;
        UI.SetActive(false);
       
    }

    private void Update()
    {
    if (isDialogueActive){
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
    }

    IEnumerator StartDialog()
    {
        isDialogueActive = true;
        objectToShow.SetActive(false);
        allowInput = false; // Disable input
        yield return new WaitForSeconds(1f);
        allowInput = true; // Enable input after the delay
        ShowNextLine();
    }

    void ShowNextLine()
    {
        if (currentLine < dialogLines.Count)
        {
            DialogLine currentDialog = dialogLines[currentLine];
            nameText.text = currentDialog.characterName;
            ChangeCharacterImage(currentLine);

            if (currentLine == 20)
            {
                StartCoroutine(ShowHideImageAndContinue());
            }
            else
            {
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
        allowInput = false; // Disable input during image display

        // Show the image
        fadeImageToShow.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f); // Wait for 5 seconds

        // Show textHideImage during image hiding
        textHideImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f); // Show textHideImage for 2 seconds

        // Hide textHideImage
        textHideImage.gameObject.SetActive(false);

        // Hide the image
        fadeImageToShow.gameObject.SetActive(false);

        allowInput = true; // Enable input after hiding the image

        // Continue to the next dialogue line
        currentLine++;
        ShowNextLine();
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
