using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemIndex;
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CollectItem();
        }
    }

    private void CollectItem()
    {
        ListItem listItemScript = FindObjectOfType<ListItem>();

        if (listItemScript != null && !listItemScript.HasItemBeenCollected(itemIndex))
        {
            listItemScript.AddItemToCollectedList(itemIndex);
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            if (collectSound != null)
            {
                audioSource.clip = collectSound;
                audioSource.Play();
            }

            Invoke("DestroyItem", 0.5f);
        }
    }

    private void DestroyItem()
    {
        Destroy(gameObject);
    }
}
