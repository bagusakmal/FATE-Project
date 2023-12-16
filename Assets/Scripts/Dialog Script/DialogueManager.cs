using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject objectToShow;
    public GameObject objectToHide;
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

    private void Start()
    {
        StartCoroutine(StartDialog());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
        yield return new WaitForSeconds(0.5f);
        ShowNextLine();
    }

    void ShowNextLine()
    {
        if (currentLine < dialogLines.Count)
        {
            DialogLine currentDialog = dialogLines[currentLine];
            nameText.text = currentDialog.characterName;
            ChangeCharacterImage(currentLine);
            StartCoroutine(TypeText(currentDialog.dialogText));
            currentLine++;
        }
        else
        {
            CloseDialog();
        }
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
    objectToHide.SetActive(false);
    }

}