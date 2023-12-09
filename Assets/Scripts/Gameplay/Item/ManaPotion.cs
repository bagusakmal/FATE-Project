using UnityEngine;

public class ManaPotion : MonoBehaviour
{
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectManaPotion();
        }
    }

    private void CollectManaPotion()
    {
        PotionManager potionManager = FindObjectOfType<PotionManager>();

        if (potionManager != null && potionManager.CollectManaPotion())
        {
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

            Invoke("DestroyPotion", 0.5f);
        }
    }

    private void DestroyPotion()
    {
        Destroy(gameObject);
    }
}
