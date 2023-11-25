using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SoundManagerScript : MonoBehaviour
{
    public Slider sfxSlider;
    public AudioSource[] sfxAudioSources;

    private SoundSettings soundSettings;

    private void Start()
    {
        LoadSoundSettings();
    }

    public void UpdateSFXVolume()
    {
        float sfxVolume = sfxSlider.value;
        soundSettings.sfxVolume = sfxVolume;

        SaveSoundSettings(); 

        foreach (AudioSource sfxSource in sfxAudioSources)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    private void LoadSoundSettings()
    {
        string json = PlayerPrefs.GetString("SoundSettings", "");
        if (!string.IsNullOrEmpty(json))
        {
            soundSettings = JsonUtility.FromJson<SoundSettings>(json);
            sfxSlider.value = soundSettings.sfxVolume;
            UpdateSFXVolume();
        }
        else
        {
            soundSettings = new SoundSettings();
        }
    }

    private void SaveSoundSettings()
    {
        string json = JsonUtility.ToJson(soundSettings);
        PlayerPrefs.SetString("SoundSettings", json);
    }
}
