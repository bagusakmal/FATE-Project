using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCreditButton : MonoBehaviour
{
    public string sceneToLoad; // Nama scene yang akan dipindahkan

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}