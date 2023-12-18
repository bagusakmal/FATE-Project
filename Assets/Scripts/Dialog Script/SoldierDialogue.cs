using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SoldierDialogue : MonoBehaviour
{
    public GameObject objectToShow;
    public GameObject hiddenGameObject;
    public bool showHiddenObjectOnStart = false;

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
    private bool isPlayerInTrigger = false;
    private PlayerControl playerControl;
    private PlayerCombatController playerCombatController;
    private bool isDialogueActive = false;

   void Start()
{
    objectToShow.SetActive(false);
    playerControl = FindObjectOfType<PlayerControl>();
    playerCombatController = FindObjectOfType<PlayerCombatController>();
    hiddenGameObject.SetActive(false);
    playerControl.enabled = false;

    if (showHiddenObjectOnStart && hiddenGameObject != null)
    {
        hiddenGameObject.SetActive(true);
    }
}

    void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        isPlayerInTrigger = true;

        if (hiddenGameObject != null)
        {
            hiddenGameObject.SetActive(true);
        }
    }
}

    void OnTriggerExit2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        isPlayerInTrigger = false;

        if (hiddenGameObject != null)
        {
            hiddenGameObject.SetActive(false);
        }
    }
}

    private void Update()
{
    if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.F))
    {
        objectToShow.SetActive(true);
        StartCoroutine(StartDialog());
    }

    if (isTyping && Input.GetMouseButtonDown(0))
    {
        cancelTyping = true;
    }
    else if (isDialogueActive)
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
}

    IEnumerator StartDialog()
    {
        isDialogueActive = true ;
        hiddenGameObject.SetActive(false);
        playerControl.enabled = false;
        playerCombatController.enabled = false;
        yield return new WaitForSeconds(1f);
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
    playerControl.enabled = true;
    playerCombatController.enabled = true;
    objectToShow.SetActive(false);
}
}
