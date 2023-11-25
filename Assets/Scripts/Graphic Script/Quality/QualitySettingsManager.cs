using UnityEngine;
using UnityEngine.UI;

public class GraphicsQualityManager : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public Toggle lowQualityToggle;
    public Toggle mediumQualityToggle;
    public Toggle highQualityToggle;

    private void Start()
    {
        int qualityLevel = PlayerPrefs.GetInt("GraphicsQuality", 2); 
        SetToggle(qualityLevel);

        toggleGroup.SetAllTogglesOff();
    }

    public void SetQuality(int qualityLevel)
    {
        QualitySettings.SetQualityLevel(qualityLevel);

        PlayerPrefs.SetInt("GraphicsQuality", qualityLevel);
        PlayerPrefs.Save();
    }

    public void SetToggle(int qualityLevel)
    {
        switch (qualityLevel)
        {
            case 0:
                lowQualityToggle.isOn = true;
                break;
            case 1:
                mediumQualityToggle.isOn = true;
                break;
            case 2:
                highQualityToggle.isOn = true;
                break;
            default:
                highQualityToggle.isOn = true;
                break;
        }
    }

    public void OnLowQualityToggle()
    {
        SetQuality(0);
    }

    public void OnMediumQualityToggle()
    {
        SetQuality(1);
    }

    public void OnHighQualityToggle()
    {
        SetQuality(2);
    }
}
