using UnityEngine;

public class PopUpCredit : MonoBehaviour
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