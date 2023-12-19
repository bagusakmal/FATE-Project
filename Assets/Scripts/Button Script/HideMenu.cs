using UnityEngine;

public class HideMenu : MonoBehaviour
{
    public GameObject popupMenu;

    
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