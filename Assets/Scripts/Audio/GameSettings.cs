using UnityEngine;

public static class GameSettings
{
    public static float MasterVolume
    {
        get { return PlayerPrefs.GetFloat("MasterVol", 1); }
        set { PlayerPrefs.SetFloat("MasterVol", value); }
    }

    public static float MusicVolume
    {
        get { return PlayerPrefs.GetFloat("MusicVol", 1); }
        set { PlayerPrefs.SetFloat("MusicVol", value); }
    }

    public static float SFXVolume
    {
        get { return PlayerPrefs.GetFloat("SFXVol", 1); }
        set { PlayerPrefs.SetFloat("SFXVol", value); }
    }
}
