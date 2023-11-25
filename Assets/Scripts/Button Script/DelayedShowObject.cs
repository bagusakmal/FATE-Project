using UnityEngine;

public class DelayedShowObject : MonoBehaviour
{
    public GameObject objectToShow;
    public float delayInSeconds = 5.0f;

    private float currentTime = 0.0f;
    private bool objectShown = false;

    void Start()
    {
        objectToShow.SetActive(false);
    }

    void Update()
    {
        if (!objectShown)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= delayInSeconds)
            {
                objectToShow.SetActive(true);
                objectShown = true;
            }
        }
    }
}
