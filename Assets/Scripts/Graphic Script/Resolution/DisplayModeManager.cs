using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayModeManager : MonoBehaviour
{
    public TMP_Dropdown displayDropdown;

    private void Start()
    {
        
        displayDropdown.ClearOptions();
        List<string> options = new List<string> { "Fullscreen", "Windowed" };
        displayDropdown.AddOptions(options);

        int savedDisplayMode = PlayerPrefs.GetInt("DisplayMode", 0);
        displayDropdown.value = savedDisplayMode;

        displayDropdown.onValueChanged.AddListener(ChangeDisplayMode);
    }

    private void ChangeDisplayMode(int index)
    {
        if (index == 0)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }

        PlayerPrefs.SetInt("DisplayMode", index);
        PlayerPrefs.Save();
    }
}
