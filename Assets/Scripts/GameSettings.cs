using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [Header("Settings")]
    public GameObject audioPanel;
    public GameObject displayPanel;
    [SerializeField] GameObject[] settingsPanels;
    public Button audioPanelButton;
    public Button displayPanelButton;
    [SerializeField] Button[] settingsButtons;
    public Button exitSettings;
    GameObject returnCanvas;

    [Header("Audio Settings")]
    public Slider mainVolume;
    public Slider soundEffectsVolume;
    public Slider musicVolume;

    [Header("Audio Sources")]
    [SerializeField] AudioSource[] soundEffectSources;
    [SerializeField] AudioSource[] musicSources;

    [Header("Display Settings")]
    public TMP_Dropdown resolutionDropdown;


    public static GameSettings instance { get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(instance.gameObject);
        else
            instance = this;

        // set resolution on awake
        Screen.SetResolution(Display.main.systemWidth, Display.main.systemHeight, true);
        int value = 0;
        foreach (TMP_Dropdown.OptionData option in resolutionDropdown.options)
        {
            string[] optionResolution = option.text.Split("x");
            if (int.Parse(optionResolution[0]) == Screen.width & int.Parse(optionResolution[1]) == Screen.height)
            {
                resolutionDropdown.value = value;
                resolutionDropdown.RefreshShownValue();
                break;
            }
            value++;
        }
    }

    void Start()
    {
        audioPanelButton.onClick.AddListener(delegate { OpenSettingsSubPanel(audioPanel, audioPanelButton); });
        displayPanelButton.onClick.AddListener(delegate { OpenSettingsSubPanel(displayPanel, displayPanelButton); });
        exitSettings.onClick.AddListener(ExitSettings);

        mainVolume.onValueChanged.AddListener(AdjustMainVolume);
        soundEffectsVolume.onValueChanged.AddListener(AdjustSoundEffectsVolume);
        musicVolume.onValueChanged.AddListener(AdjustMusicVolume);

        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        OpenSettingsSubPanel(audioPanel, audioPanelButton);
        gameObject.SetActive(false);
    }

    public void OpenSettings(GameObject rtrnCanvas = null)
    {
        if (rtrnCanvas == null)
            GetComponent<Image>().enabled = true;
        else
            GetComponent<Image>().enabled = false;

            returnCanvas = rtrnCanvas;
        gameObject.SetActive(true);

        OpenSettingsSubPanel(audioPanel, audioPanelButton);
    }

    public void OpenSettingsSubPanel(GameObject panel, Button navBtn = null)
    {
        foreach (GameObject p in settingsPanels)
        {
            if (p == panel)
                p.gameObject.SetActive(true);
            else
                p.gameObject.SetActive(false);
        }

        if (navBtn)
        {
            foreach (Button btn in settingsButtons)
            {
                if (btn == navBtn)
                    btn.interactable = false;
                else
                    btn.interactable = true;
            }
        }
    }

    void ExitSettings()
    {
        gameObject.SetActive(false);

        if (returnCanvas != null)
            returnCanvas.SetActive(true);
        else
            Time.timeScale = 1f;
    }

    // TODO general settings

    /*
     * DISPLAY
     */
    public void SetResolution(int index)
    {
        string[] resolution = resolutionDropdown.options[index].text.Split('x');
        Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), true);
    }

    /*
     * AUDIO
     */
    public void AdjustMainVolume(float newVol)
    {
        foreach (AudioSource soundEffect in soundEffectSources)
        {
            soundEffect.volume = soundEffectsVolume.value * newVol;
        }

        foreach (AudioSource music in musicSources)
        {
            music.volume = musicVolume.value * newVol;
        }
    }

    public void AdjustSoundEffectsVolume(float newVol)
    {
        foreach (AudioSource soundEffect in soundEffectSources)
        {
            soundEffect.volume = mainVolume.value * newVol;
        }
    }

    public void AdjustMusicVolume(float newVol)
    {
        foreach (AudioSource music in musicSources)
        {
            music.volume = mainVolume.value * newVol;
        }
    }
}
