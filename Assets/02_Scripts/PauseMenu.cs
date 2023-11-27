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
    [SerializeField] float currentVolume;
    [SerializeField] public GameObject optionsMenu;
    [SerializeField] GameObject saveButton;
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

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("VolumePref", currentVolume);
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void loadSettings()
    {
        if (PlayerPrefs.HasKey("VolumePref"))
        {
            VolumeSlider.value = currentVolume = PlayerPrefs.GetFloat("VolumePref");
        }
        else
        {
            VolumeSlider.value = PlayerPrefs.GetFloat("VolumePref");
        }
    }
}
