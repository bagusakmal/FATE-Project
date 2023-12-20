using UnityEngine;

public class ShowAfterDelay : MonoBehaviour
{
    public float delayInSeconds = 3f;

    private void Start()
    {
        // Memulai fungsi untuk menampilkan GameObject setelah jeda tertentu
        Invoke("ShowObject", delayInSeconds);
    }

    private void ShowObject()
    {
        // Menampilkan GameObject
        gameObject.SetActive(true);
    }
}
