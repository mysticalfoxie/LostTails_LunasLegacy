using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")] 
    [SerializeField] private string _fileName;

    [SerializeField] private bool _useEncryption;

    private GameData _gameData;
    private List<DataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in this scene.");
        }

        Instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, _useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
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
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        foreach (DataPersistence dataPersistence in dataPersistenceObjects)
        {
            dataPersistence.LoadData(_gameData);
        }
        
    }

    public void SaveGame()
    {
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
}
