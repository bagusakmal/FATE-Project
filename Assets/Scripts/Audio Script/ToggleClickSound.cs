using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleClickSound : MonoBehaviour
{
    public List<Toggle> toggles;
    public AudioSource clickSound; 

    void Start()
    {
        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener(PlayClickSound);
        }
    }

    void PlayClickSound(bool isOn)
    {
        if (isOn)
        {
            clickSound.Play();
        }
    }
}
