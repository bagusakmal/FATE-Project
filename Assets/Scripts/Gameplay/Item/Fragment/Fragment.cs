using UnityEngine;

public class Fragment : MonoBehaviour
{
    public int fragmentIndex;
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            CollectFragment();
        }
    }

    private void CollectFragment()
    {
        ListFragment listFragmentScript = FindObjectOfType<ListFragment>();

        if (listFragmentScript != null && !listFragmentScript.HasItemBeenCollected(fragmentIndex))
        {
            listFragmentScript.AddItemToCollectedList(fragmentIndex);
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

            Invoke("DestroyFragment", 0.5f);
        }
    }

    private void DestroyFragment()
    {
        Destroy(gameObject);
    }
}
