using System;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider musicVolSlider;
    [SerializeField] private Slider sfxVolSlider;

    private void Start()
    {
        masterVolSlider.value = GameSettings.MasterVolume;
        AudioManager.Instance.SetMixerVolume("MasterVol", masterVolSlider.value);
        masterVolSlider.onValueChanged.AddListener(OnMasterVolumeChanged);

        musicVolSlider.value = GameSettings.MusicVolume;
        AudioManager.Instance.SetMixerVolume("MusicVol", musicVolSlider.value);
        musicVolSlider.onValueChanged.AddListener(OnMusicVolumeChanged);

        sfxVolSlider.value = GameSettings.SFXVolume;
        AudioManager.Instance.SetMixerVolume("SFXVol", sfxVolSlider.value);
        sfxVolSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnSFXVolumeChanged(float arg0)
    {
        GameSettings.SFXVolume = arg0;
        AudioManager.Instance.SetMixerVolume("SFXVol", GameSettings.SFXVolume);
    }

    private void OnMusicVolumeChanged(float arg0)
    {
        GameSettings.MusicVolume = arg0;
        AudioManager.Instance.SetMixerVolume("MusicVol", GameSettings.MusicVolume);
    }

    private void OnMasterVolumeChanged(float arg0)
    {
        GameSettings.MasterVolume = arg0;
        AudioManager.Instance.SetMixerVolume("MasterVol", GameSettings.MasterVolume);
    }
}