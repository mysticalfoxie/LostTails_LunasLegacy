using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")] 
    [SerializeField] private string _fileName;
    private GameData _gameData;
    private GameManager _gameManager;
    private FileDataHandler _dataHandler;
    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {       
        if (Instance is not null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName);
        _gameData = _dataHandler?.Load();
    }

    public void NewGame()
    {
        _gameData = new GameData();
    }

    public int? GetLevelIndex() => _dataHandler.Load()?.SavedLevelIndex;

    public bool HasGameData() => !new[] { 0, 11 }.Contains(_gameData?.SavedLevelIndex ?? 0);

    public void UpdateLevelIndex(int index)
    {
        _gameData ??= new GameData();
        
        /* ??=  -> Shortcut for this:
            if (_gameData == null)
                _gameData == new GameData();
        */
        
        _gameData.SavedLevelIndex = index;
        if (Debug.isDebugBuild)
            Debug.Log(JsonUtility.ToJson(_gameData));
        _dataHandler.Save(_gameData);
    }
}
