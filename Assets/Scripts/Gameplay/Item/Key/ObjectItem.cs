using UnityEngine;

public class ObjectItem : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.OnObjectDestroyed(gameObject.name);
        }
    }
}
