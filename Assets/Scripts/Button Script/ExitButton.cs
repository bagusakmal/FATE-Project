using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    
    public Button exitButton;

    void Start()
    {
        
        exitButton.onClick.AddListener(ExitGame);
    }
    
    void ExitGame()
    {
        
        Application.Quit();
    }
}
