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
    [SerializeField] float currentVolume;
    [SerializeField] float currentMusicVolume;
    [SerializeField] float currentSFXVolume;
    [SerializeField] public GameObject optionsMenu;
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject saveButton;
    [SerializeField] GameObject controlsButton;
    [SerializeField] GameObject backButton;
    [SerializeField] GameObject optionsButton;
    [SerializeField] GameObject pauseMenu;
    bool Paused = false;

    private void Start()
    {
        loadSettings();
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
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("VolumePref", currentVolume);
        PlayerPrefs.SetFloat("MusicPref", currentMusicVolume);
        PlayerPrefs.SetFloat("SFXPref", currentSFXVolume);
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void BackButton()
    {
        pauseMenu.SetActive(true);
        controlsMenu.SetActive(false);
    }
    public void loadSettings()
    {
        if (PlayerPrefs.HasKey("VolumePref")) VolumeSlider.value = currentVolume = PlayerPrefs.GetFloat("VolumePref");
        else VolumeSlider.value = PlayerPrefs.GetFloat("VolumePref");

        if (PlayerPrefs.HasKey("MusicPref")) MusicSlider.value = currentMusicVolume = PlayerPrefs.GetFloat("MusicPref");
        else MusicSlider.value = PlayerPrefs.GetFloat("MusicPref");

        if (PlayerPrefs.HasKey("SFXPref")) SFXSlider.value = currentSFXVolume = PlayerPrefs.GetFloat("SFXPref");
        else SFXSlider.value = PlayerPrefs.GetFloat("SFXPref");
    }
}
