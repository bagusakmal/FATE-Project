using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlightSound : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource soundManager;
    public AudioClip highlightSound;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        soundManager.PlayOneShot(highlightSound);
    }
}
