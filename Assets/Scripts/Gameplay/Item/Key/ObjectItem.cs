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
            Debug.Log("item delet");
            gameManager.OnObjectDestroyed(gameObject.name);
        }
    }
}
