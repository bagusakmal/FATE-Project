using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoKey : MonoBehaviour
{
    public GameObject hideInfo;
    private bool isPlayerInTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // You can add any update logic here if needed
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            hideInfo.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting collider is the player
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            hideInfo.SetActive(false);
        }
    }
}
