using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class StartMenu : MonoBehaviour
{
    [Header("Music Control")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider VolumeSlider;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] float currentVolume;
    [SerializeField] float currentMusicVolume;
    [SerializeField] float currentSFXVolume;
    [SerializeField] AudioSource backgroundAudio;

    [Header("Dropdowns")]
    [SerializeField] Dropdown resolutionDropdown;
    [SerializeField] Dropdown qualityDropdown;
    [SerializeField] Dropdown textureDropdown;
    [SerializeField] Dropdown aaDropdown;

    Resolution[] resolutions;

    [Header("Menus")]
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject creditsMenu;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject stateMenu;

    [Header("Buttons")]
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject creditsButton;
    [SerializeField] GameObject controlsButton;
    [SerializeField] GameObject optionsButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] GameObject saveButton;
    [SerializeField] GameObject backButton;

    Fading fading;
    public void Start()
    {
        backgroundAudio = FindObjectOfType<AudioSource>();
        backgroundAudio.Play();
        fading = FindAnyObjectByType<Fading>();
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
    public void StartGame()
    {
        StartCoroutine(_ChangeScene());
    }

    public IEnumerator _ChangeScene()
    {
        fading.FadeIn();
        backgroundAudio.Stop();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
        fading.FadeOut();
    }

    public void Credits()
    {
        startMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void Controls()
    {
        startMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void Options() //To-Do: Sound Slider!
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
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
    public void QuitGame()
    {
        Application.Quit();
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

    private bool qualitylocked;
    public void SetQuality(int qualityIndex)
    {
        if (qualitylocked == true) return;
        if (qualityIndex == 6) return;
        qualitylocked = true;
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
        qualitylocked = false;
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("VolumePref", currentVolume);
        PlayerPrefs.SetFloat("MusicPref", currentMusicVolume);
        PlayerPrefs.SetFloat("SFXPref", currentSFXVolume);
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("TextureQualityPreference", textureDropdown.value);
        PlayerPrefs.SetInt("AntiAliasingPreference",aaDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference",Convert.ToInt32(Screen.fullScreen));
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void BackButton()
    {
        startMenu.SetActive(true);
        creditsMenu.SetActive(false);
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

        if(PlayerPrefs.HasKey("VolumePref"))VolumeSlider.value = currentVolume = PlayerPrefs.GetFloat("VolumePref");
        else VolumeSlider.value = PlayerPrefs.GetFloat("VolumePref");

        if (PlayerPrefs.HasKey("MusicPref")) MusicSlider.value = currentMusicVolume = PlayerPrefs.GetFloat("MusicPref");
        else MusicSlider.value = PlayerPrefs.GetFloat("MusicPref");

        if (PlayerPrefs.HasKey("SFXPref")) SFXSlider.value = currentSFXVolume = PlayerPrefs.GetFloat("SFXPref");
        else SFXSlider.value = PlayerPrefs.GetFloat("SFXPref");
    }
}
