using System.Collections;
using UnityEngine;

public class PlayerOneWayPlatform : MonoBehaviour
{
    private Collider2D[] currentOneWayPlatformColliders;

    [SerializeField] private Collider2D playerCollider;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentOneWayPlatformColliders != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatformColliders = collision.gameObject.GetComponents<Collider2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            // Tidak perlu mengatur null di sini
        }
    }

    private IEnumerator DisableCollision()
    {
        foreach (Collider2D platformCollider in currentOneWayPlatformColliders)
        {
            if (platformCollider != null && platformCollider.CompareTag("OneWayPlatform"))
            {
                Physics2D.IgnoreCollision(playerCollider, platformCollider);
            }
            else
            {
                Debug.LogWarning("One of the colliders in currentOneWayPlatformColliders is null or does not have the 'OneWayPlatform' tag.");
            }
        }

        yield return new WaitUntil(() => !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.DownArrow));

        foreach (Collider2D platformCollider in currentOneWayPlatformColliders)
        {
            if (platformCollider != null && platformCollider.CompareTag("OneWayPlatform"))
            {
                Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
            }
            else
            {
                Debug.LogWarning("One of the colliders in currentOneWayPlatformColliders is null or does not have the 'OneWayPlatform' tag.");
            }
        }
    }
}
