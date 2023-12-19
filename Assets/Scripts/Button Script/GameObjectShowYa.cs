using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameObjectShowYa : MonoBehaviour
{
    public GameObject myGameObject;
    public float delayTime = 3f; // Ubah sesuai dengan waktu yang diinginkan sebelum game object muncul

    private float timer = 0f;
    private bool isHidden = true;

    void Update()
{
    // Update timer
    timer += Time.deltaTime;

    // Cek apakah sudah waktunya untuk menampilkan game object
    if (timer >= delayTime && isHidden)
    {
        Debug.Log("Showing GameObject!");
        ShowGameObject();
    }
}

    void ShowGameObject()
    {
        // Tampilkan game object
        myGameObject.SetActive(true);

        // Setel status isHidden menjadi false agar tidak diproses lagi
        isHidden = false;
    }
}
