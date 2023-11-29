using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class StartMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider VolumeSlider;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] float currentVolume;
    [SerializeField] float currentMusicVolume;
    [SerializeField] float currentSFXVolume;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject creditsMenu;
    [SerializeField] GameObject startMenu;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject creditsButton;
    [SerializeField] GameObject controlsButton;
    [SerializeField] GameObject optionsButton;
    [SerializeField] GameObject quitButton;
    [SerializeField] GameObject saveButton;
    [SerializeField] GameObject backButton;

    //To-Do: How To Play?, Options Button, Menu Sound?

    public void Start()
    {
        loadSettings();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
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
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("VolumePref", currentVolume);
        PlayerPrefs.SetFloat("MusicPref", currentMusicVolume);
        PlayerPrefs.SetFloat("SFXPref", currentSFXVolume);
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void BackButton()
    {
        startMenu.SetActive(true);
        creditsMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }
    public void loadSettings()
    {
        if(PlayerPrefs.HasKey("VolumePref"))VolumeSlider.value = currentVolume = PlayerPrefs.GetFloat("VolumePref");
        else VolumeSlider.value = PlayerPrefs.GetFloat("VolumePref");

        if (PlayerPrefs.HasKey("MusicPref")) MusicSlider.value = currentMusicVolume = PlayerPrefs.GetFloat("MusicPref");
        else MusicSlider.value = PlayerPrefs.GetFloat("MusicPref");

        if (PlayerPrefs.HasKey("SFXPref")) SFXSlider.value = currentSFXVolume = PlayerPrefs.GetFloat("SFXPref");
        else SFXSlider.value = PlayerPrefs.GetFloat("SFXPref");
    }
}
