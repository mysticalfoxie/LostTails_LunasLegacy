using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool _initializeDataIfNull;
    [Header("File Storage Config")] 
    [SerializeField] private string _fileName;
    private GameData _gameData;
    private GameManager _gameManager;
    private List<IDataPersistence> _dataPersistenceObjects;
    private FileDataHandler _dataHandler;
    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in this scene.");
            Destroy	(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName);
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
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    private void Start()
    {
        _dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public void LoadGame()
    {
        _gameData = _dataHandler.Load();

        if (_gameData == null && _initializeDataIfNull)
        {
            NewGame();
        }
        
        if (_gameData == null)
        {
            Debug.Log("No data was found. A new game needs to be started before data can be loeaded.");
            return;
        }

        foreach (IDataPersistence dataPersistence in _dataPersistenceObjects)
        {
            dataPersistence.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        if (_gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved");
            return;
        }
        foreach (IDataPersistence dataPersistence in _dataPersistenceObjects)
        {
            dataPersistence.SaveData(ref _gameData);
        }
        _dataHandler.Save(_gameData);
            
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects =
            FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects); 
    }

    public bool HasGameData()
    {
        return _gameData != null;
    }
}
