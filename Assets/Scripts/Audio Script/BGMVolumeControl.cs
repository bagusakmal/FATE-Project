using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMVolumeControl : MonoBehaviour
{
    public Slider bgmVolumeSlider;
    public AudioSource bgmAudioSource;

    private void Start()
    {
        
        float savedVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        bgmVolumeSlider.value = savedVolume;
        
        SetBGMVolume(savedVolume);
    }

    public void SetBGMVolume(float volume)
    {
        
        bgmAudioSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

}
