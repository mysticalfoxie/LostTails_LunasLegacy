using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [Header("Music Control")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private float _currentVolume;
    [SerializeField] private float _currentMusicVolume;
    [SerializeField] private float _currentSfxVolume;
    [SerializeField] private AudioSource _backgroundAudio;

    [Header("Dropdowns")]
    [SerializeField] private Dropdown _resolutionDropdown;
    [SerializeField] private Dropdown _qualityDropdown;
    [SerializeField] private Dropdown _textureDropdown;
    [SerializeField] private Dropdown _aaDropdown;

    private Resolution[] _resolutions;

    [Header("Menus")]
    [SerializeField] private GameObject _optionsMenu;
    [SerializeField] private GameObject _controlsMenu;
    [SerializeField] private GameObject _creditsMenu;
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _endScene;
    private bool _paused;
    private bool _gameStarted;
    
    [SerializeField] private GameObject _loadGameButton;
    
    [SerializeField] private GameObject _backgroundImage;

    private Boolean _blockPauseMenu = true;
    
    private bool _starting;
    // ReSharper disable once HeapView.ObjectAllocation
    private readonly int[] _cutScenes = {0,1,3,5,10};
    private PlayerMovement _playerMovement;

    public void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode uwu)
    {
        if (GameManager.Instance._currentLevelIndex != 11) return;
        if (_pauseMenu) _pauseMenu.SetActive(false);
        if (_startMenu) _startMenu.SetActive(false);
        if (!_endScene) _endScene.SetActive(true);
    }

    public void Start()
    {
        _startMenu.SetActive(true);
        _backgroundImage.SetActive(true);
        _backgroundAudio = FindObjectOfType<AudioSource>();
        _backgroundAudio.Play();
        _resolutionDropdown.ClearOptions();
        DataPersistenceManager.Instance.HasGameData();
        var hasGameData = DataPersistenceManager.Instance.HasGameData();
        _loadGameButton.SetActive(hasGameData);

        var options = new List<string>();
        _resolutions = Screen.resolutions;
        var currentResolutionIndex = 0;
        for (var i = 0; i < _resolutions.Length; i++)
        {
            var option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);
            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
    }

    public void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape) || _blockPauseMenu) return;
        if (_playerMovement is { isJumping: true }) return;
        if (_cutScenes.Contains(GameManager.Instance._currentLevelIndex) ) return;
        
        if (!_paused)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    public void StartGame()
    {
        if (_starting) return;
        _starting = true;
        _blockPauseMenu = false;
        DataPersistenceManager.Instance.NewGame();
        StartCoroutine(_ChangeScene());
    }

    public void LoadGame()
    {
        if (_starting) return;
        _starting = true;
        _blockPauseMenu = false;
        StartCoroutine(_ChangeScene(GameManager.Instance._currentLevelIndex));    
    }

    private void Pause()
    {
        Time.timeScale = 0.0f;
        _pauseMenu.SetActive(true);
        _paused = true;
    }

    public void Resume()
    {
        // Cursor.visible = false;
        Time.timeScale = 1.0f;
        _pauseMenu.SetActive(false);
        _paused = false;
    }

    private IEnumerator _ChangeScene(int? index = null)
    {
        yield return index.HasValue
            ? GameManager.LoadLevelAsync(index.Value, 1.0F, 1.0F, ActionsDuringSceneChange)
            : GameManager.LoadNextLevelAsync(1.0F, 1.0F, ActionsDuringSceneChange);
        
        _starting = false;
    }
    
    [SuppressMessage("ReSharper", "Unity.NoNullPropagation")]
    private IEnumerator ActionsDuringSceneChange()
    {
        _backgroundAudio?.Stop();
        if (_gameStarted)
            yield break;
        
        _backgroundImage?.SetActive(false);
        _startMenu?.SetActive(false);
        _gameStarted = true;
    }

    public void Credits()
    {
        GameStartedOff();
        _creditsMenu.SetActive(true);
    }

    public void Controls()
    {
        GameStartedOff();
        _controlsMenu.SetActive(true);
    }

    public void Home()
    {
        if (Time.timeScale == 0) Time.timeScale = 1;
        SceneManager.LoadScene(0);
        _gameStarted = false;
        GameStartedOn();
        if(_pauseMenu) _pauseMenu.SetActive(false);
        if(_endScene) _endScene.SetActive(false);
        _blockPauseMenu = true;
    }

    public void Options()
    {
        GameStartedOff();
        _optionsMenu.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        _audioMixer.SetFloat("Volume", volume);
        _currentVolume = volume;
    }

    public void SetMusic(float music)
    {
        _audioMixer.SetFloat("Music", music);
        _currentMusicVolume = music;
    }

    public void SetSfx(float sfx)
    {
        _audioMixer.SetFloat("SFX", sfx);
        _currentSfxVolume = sfx;
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
        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        _qualityDropdown.value = 6;
    }

    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        _qualityDropdown.value = 6;
    }

    private bool _qualitylocked;


    public void SetQuality(int qualityIndex)
    {
        if (_qualitylocked) return;
        if (qualityIndex == 6) return;
        _qualitylocked = true;
        QualitySettings.SetQualityLevel(qualityIndex);
        switch (qualityIndex)
        {
            case 0: // quality level - very low
                _textureDropdown.value = 3;
                _aaDropdown.value = 0;
                break;
            case 1: // quality level - low
                _textureDropdown.value = 2;
                _aaDropdown.value = 0;
                break;
            case 2: // quality level - medium
                _textureDropdown.value = 1;
                _aaDropdown.value = 0;
                break;
            case 3: // quality level - high
                _textureDropdown.value = 0;
                _aaDropdown.value = 0;
                break;
            case 4: // quality level - very high
                _textureDropdown.value = 0;
                _aaDropdown.value = 1;
                break;
            case 5: // quality level - ultra
                _textureDropdown.value = 0;
                _aaDropdown.value = 2;
                break;
        }

        _qualityDropdown.value = qualityIndex;
        _qualitylocked = false;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("VolumePref", _currentVolume);
        PlayerPrefs.SetFloat("MusicPref", _currentMusicVolume);
        PlayerPrefs.SetFloat("SFXPref", _currentSfxVolume);
        PlayerPrefs.SetInt("QualitySettingPreference", _qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", _resolutionDropdown.value);
        PlayerPrefs.SetInt("TextureQualityPreference", _textureDropdown.value);
        PlayerPrefs.SetInt("AntiAliasingPreference", _aaDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(Screen.fullScreen));
        GameStartedOn();
        _optionsMenu.SetActive(false);
    }

    public void BackButton()
    {
        GameStartedOn();
        _creditsMenu.SetActive(false);
        _controlsMenu.SetActive(false);
    }

    private void GameStartedOn()
    {
        if (_gameStarted)
        {
            _pauseMenu.SetActive(true);
        }
        else
        {
            _startMenu.SetActive(true);
            _backgroundImage.SetActive(true);
        }
    }

    private void GameStartedOff()
    {
        if (_gameStarted)
        {
            _pauseMenu.SetActive(false);
        }
        else
        {
            _startMenu.SetActive(false);
            if (_gameStarted) _backgroundImage.SetActive(false);
        }
    }
    private void LoadSettings(int currentResolutionIndex)
    {
        _qualityDropdown.value = PlayerPrefs.HasKey("QualitySettingPreference") ? PlayerPrefs.GetInt("QualitySettingPreference") : 3;

        _resolutionDropdown.value = PlayerPrefs.HasKey("ResolutionPreference") ? PlayerPrefs.GetInt("ResolutionPreference") : currentResolutionIndex;

        _textureDropdown.value = PlayerPrefs.HasKey("TextureQualityPreference") ? PlayerPrefs.GetInt("TextureQualityPreference") : 0;

        _aaDropdown.value = PlayerPrefs.HasKey("AntiAliasingPreference") ? PlayerPrefs.GetInt("AntiAliasingPreference") : 1;

        Screen.fullScreen = !PlayerPrefs.HasKey("FullScreenPreference") || Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));

        _volumeSlider.value = PlayerPrefs.HasKey("VolumePref")
            ? _currentVolume = PlayerPrefs.GetFloat("VolumePref")
            : PlayerPrefs.GetFloat("VolumePref"); 
        
        _musicSlider.value = PlayerPrefs.HasKey("MusicPref")
            ? _currentMusicVolume = PlayerPrefs.GetFloat("MusicPref")
            : PlayerPrefs.GetFloat("MusicPref");
        
        _sfxSlider.value = PlayerPrefs.HasKey("SFXPref")
            ? _currentSfxVolume = PlayerPrefs.GetFloat("SFXPref")
            : PlayerPrefs.GetFloat("SFXPref"); 
    }
}