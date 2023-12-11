using UnityEngine;

public class HpPotion : MonoBehaviour
{
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectHpPotion();
        }
    }

    private void CollectHpPotion()
    {
        PotionManager potionManager = FindObjectOfType<PotionManager>();

        if (potionManager != null && potionManager.CollectHpPotion())
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
