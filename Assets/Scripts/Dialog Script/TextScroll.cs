using UnityEngine;
using TMPro;
using System.Collections;

public class TextScroll : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float scrollSpeed = 2f;

    private void Start()
    {
        // Memulai fungsi untuk menggulung teks
        StartCoroutine(ScrollText());
    }

    IEnumerator ScrollText()
    {
        float height = textMeshPro.preferredHeight;

        while (true)
        {
            // Posisi awal teks di luar layar bawah
            textMeshPro.rectTransform.anchoredPosition = new Vector2(0, -height);

            while (textMeshPro.rectTransform.anchoredPosition.y < 0)
            {
                // Menggerakkan teks ke atas
                textMeshPro.rectTransform.anchoredPosition += new Vector2(0, scrollSpeed) * Time.deltaTime;

                yield return null;
            }

            // Jeda sebentar sebelum memulai kembali
            yield return new WaitForSeconds(1f);
        }
    }
}
