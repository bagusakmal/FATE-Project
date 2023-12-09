using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameObjectSwitcher : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public GameObject[] gameObjects;

    private int currentIndex = 0;

    void Start()
    {
        
        Toggle[] toggles = toggleGroup.GetComponentsInChildren<Toggle>();
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate { OnToggleValueChanged(toggle); });
        }

        ShowCurrentGameObject();
    }

    void OnToggleValueChanged(Toggle changedToggle)
    {
        if (changedToggle.isOn)
        {
            int selectedIndex = System.Array.IndexOf(toggleGroup.GetComponentsInChildren<Toggle>(), changedToggle);

            currentIndex = selectedIndex;
            ShowCurrentGameObject();
        }
    }

    void ShowCurrentGameObject()
    {
        foreach (GameObject go in gameObjects)
        {
            go.SetActive(false);
        }

        if (currentIndex >= 0 && currentIndex < gameObjects.Length)
        {
            gameObjects[currentIndex].SetActive(true);
        }
    }

    public void NextGameObject()
    {
        currentIndex = (currentIndex + 1) % gameObjects.Length;

        ShowCurrentGameObject();
    }

    public void PreviousGameObject()
    {
        currentIndex = (currentIndex - 1 + gameObjects.Length) % gameObjects.Length;

        ShowCurrentGameObject();
    }
}
