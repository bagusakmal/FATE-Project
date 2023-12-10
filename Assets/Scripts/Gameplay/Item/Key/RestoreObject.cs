using UnityEngine;
using TMPro;

public class RestoreObject : MonoBehaviour
{
    public TMP_Text statusText; 
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        UpdateStatusText();
    }

    public void RestoreGameObject()
    {
        string[] destroyedObjectsArray = PlayerPrefs.GetString(GameManager.DestroyedObjectsKey, "").Split(',');

        foreach (string objectName in destroyedObjectsArray)
        {
            if (!string.IsNullOrEmpty(objectName))
            {
                GameObject destroyedObject = GameObject.Find(objectName);
                if (destroyedObject != null)
                {
                    destroyedObject.SetActive(true);
                }
            }
        }

        PlayerPrefs.SetString(GameManager.DestroyedObjectsKey, "");
        PlayerPrefs.Save();

        UpdateStatusText();
    }

    private void UpdateStatusText()
    {
        if (statusText != null)
        {
            string status = PlayerPrefs.GetString(GameManager.DestroyedObjectsKey, "").Length > 0
                ? "Objects Destroyed"
                : "No Objects Destroyed";
            statusText.text = "Status: " + status;
        }
    }
}
