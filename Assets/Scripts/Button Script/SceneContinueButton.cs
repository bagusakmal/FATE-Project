using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneContinueButton : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}