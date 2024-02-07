using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")] 
    [SerializeField] private string _fileName;

    [SerializeField] private bool _useEncryption;

    private GameData _gameData;
    private GameManager _gameManager;
   // private int saved_level = _gameManager.savedLevelIndex;
    private List<DataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in this scene.");
            Destroy	(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    public void NewGame()
    {
        this._gameData = gameObject.AddComponent<GameData>();
    }

    public void LoadGame()
    {
        this._gameData = dataHandler.Load();
        if (this._gameData == null)
        {
            Debug.Log("No data was found. A new game needs to be started before data can be loeaded.");
            return;
        }

        foreach (DataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.LoadData(_gameData);
        }

      //  saved_level = _gameManager.currentLevelIndex(buildIndex);

    }

    public void SaveGame()
    {
        if (this._gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved");
            return;
        }
        foreach (DataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.SaveData(ref _gameData);
        }
        dataHandler.Save(_gameData);
            
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<DataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<DataPersistence> dataPersistenceObjects =
            FindObjectsOfType<MonoBehaviour>().OfType<DataPersistence>();
        return new List<DataPersistence>(dataPersistenceObjects); 

    }

    public bool HasGameData()
    {
        return _gameData != null;
    }
}
