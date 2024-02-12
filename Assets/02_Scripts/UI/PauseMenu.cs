using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public AudioMixer audioMixer;
    [SerializeField] Slider VolumeSlider;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Dropdown resolutionDropdown;
    [SerializeField] Dropdown qualityDropdown;
    [SerializeField] Dropdown textureDropdown;
    [SerializeField] Dropdown aaDropdown;
    [SerializeField] float currentVolume;
    [SerializeField] float currentMusicVolume;
    [SerializeField] float currentSFXVolume;
    Resolution[] resolutions;
    [SerializeField] public GameObject optionsMenu;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject saveButton;
    [SerializeField] GameObject controlsButton;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject optionsButton;
    bool Paused = false;

    private void Start()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        loadSettings(currentResolutionIndex);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (Paused == false)
            {
                Pause();

            } else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
       // Cursor.visible = true;
        Time.timeScale = 0.0f;
        pauseMenu.SetActive(true);
        Paused = true;
    }
    public void Resume()
    {
       // Cursor.visible = false;
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
        Paused = false;
    }

    public void Options()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void Controls()
    {
        pauseMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void Home()
    {
        if (Time.timeScale == 0) Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        currentVolume = volume;
    }
    public void SetMusic(float music)
    {
        audioMixer.SetFloat("Music", music);
        currentMusicVolume = music;
    }
    public void SetSFX(float sfx)
    {
        audioMixer.SetFloat("SFX", sfx);
        currentSFXVolume = sfx;
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        qualityDropdown.value = 6;
    }

    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        qualityDropdown.value = 6;
    }

    public void SetQuality(int qualityIndex)
    {
        if (qualityIndex != 6)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
            switch (qualityIndex)
            {
                case 0: // quality level - very low
                    textureDropdown.value = 3;
                    aaDropdown.value = 0;
                    break;
                case 1: // quality level - low
                    textureDropdown.value = 2;
                    aaDropdown.value = 0;
                    break;
                case 2: // quality level - medium
                    textureDropdown.value = 1;
                    aaDropdown.value = 0;
                    break;
                case 3: // quality level - high
                    textureDropdown.value = 0;
                    aaDropdown.value = 0;
                    break;
                case 4: // quality level - very high
                    textureDropdown.value = 0;
                    aaDropdown.value = 1;
                    break;
                case 5: // quality level - ultra
                    textureDropdown.value = 0;
                    aaDropdown.value = 2;
                    break;
            }
            qualityDropdown.value = qualityIndex;
        }
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("VolumePref", currentVolume);
        PlayerPrefs.SetFloat("MusicPref", currentMusicVolume);
        PlayerPrefs.SetFloat("SFXPref", currentSFXVolume);
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("TextureQualityPreference", textureDropdown.value);
        PlayerPrefs.SetInt("AntiAliasingPreference", aaDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(Screen.fullScreen));
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void BackButton()
    {
        pauseMenu.SetActive(true);
        controlsMenu.SetActive(false);
    }
    public void loadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference")) qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        else qualityDropdown.value = 3;

        if (PlayerPrefs.HasKey("ResolutionPreference")) resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else resolutionDropdown.value = currentResolutionIndex;

        if (PlayerPrefs.HasKey("TextureQualityPreference")) textureDropdown.value = PlayerPrefs.GetInt("TextureQualityPreference");
        else textureDropdown.value = 0;

        if (PlayerPrefs.HasKey("AntiAliasingPreference")) aaDropdown.value = PlayerPrefs.GetInt("AntiAliasingPreference");
        else aaDropdown.value = 1;

        if (PlayerPrefs.HasKey("FullScreenPreference")) Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else Screen.fullScreen = true;

        if (PlayerPrefs.HasKey("VolumePref")) VolumeSlider.value = currentVolume = PlayerPrefs.GetFloat("VolumePref");
        else VolumeSlider.value = PlayerPrefs.GetFloat("VolumePref");

        if (PlayerPrefs.HasKey("MusicPref")) MusicSlider.value = currentMusicVolume = PlayerPrefs.GetFloat("MusicPref");
        else MusicSlider.value = PlayerPrefs.GetFloat("MusicPref");

        if (PlayerPrefs.HasKey("SFXPref")) SFXSlider.value = currentSFXVolume = PlayerPrefs.GetFloat("SFXPref");
        else SFXSlider.value = PlayerPrefs.GetFloat("SFXPref");
    }
}
