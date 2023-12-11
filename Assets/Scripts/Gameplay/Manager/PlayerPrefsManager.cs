using UnityEngine;
using TMPro;

public class PlayerPrefsManager : MonoBehaviour
{
    // Pilih button TMPRO pada Inspector dan tambahkan fungsi ini ke event OnClick
    public void OnClearPlayerPrefsButtonClick()
    {
        ClearAllPlayerPrefs();
    }

    // Fungsi untuk menghapus semua data di PlayerPrefs
    private void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Semua data di PlayerPrefs telah dihapus.");
    }
}
