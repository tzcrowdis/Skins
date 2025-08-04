using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    string mainVolumeKey = "MainVolume";
    string soundEffectsVolumeKey = "SoundEffectsVolume";
    string musicVolumeKey = "MusicVolume";

    string resolutionKey = "Resolution";

    
    public static SaveManager instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        LoadPlayerSettings();
    }

    public void SavePlayerSettings()
    {
        PlayerPrefs.SetFloat(mainVolumeKey, GameSettings.instance.mainVolume.value);
        PlayerPrefs.SetFloat(soundEffectsVolumeKey, GameSettings.instance.soundEffectsVolume.value);
        PlayerPrefs.SetFloat(musicVolumeKey, GameSettings.instance.musicVolume.value);

        PlayerPrefs.SetInt(resolutionKey, GameSettings.instance.resolutionDropdown.value);
    }

    void LoadPlayerSettings()
    {
        if (PlayerPrefs.HasKey(mainVolumeKey)) GameSettings.instance.mainVolume.value = PlayerPrefs.GetFloat(mainVolumeKey);
        if (PlayerPrefs.HasKey(soundEffectsVolumeKey)) GameSettings.instance.soundEffectsVolume.value = PlayerPrefs.GetFloat(soundEffectsVolumeKey);
        if (PlayerPrefs.HasKey(musicVolumeKey)) GameSettings.instance.musicVolume.value = PlayerPrefs.GetFloat(musicVolumeKey);

        if (PlayerPrefs.HasKey(resolutionKey)) GameSettings.instance.SetResolution(PlayerPrefs.GetInt(resolutionKey));
    }
}
