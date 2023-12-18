using UnityEngine;

public class HideDialogue2 : MonoBehaviour
{
    public GameObject popupMenu;

    // Check for Escape key press
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HidePopup();
        }
    }

    public void ShowPopup()
    {
        if (popupMenu != null)
        {
            popupMenu.SetActive(true);
        }
    }

    public void HidePopup()
    {
        if (popupMenu != null)
        {
            popupMenu.SetActive(false);
        }
    }
}
