using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class GameData
{    
    public Vector3 _playerPosition;
    public int _savedLevelIndex;

    private void Start()
    {
        
    }
    public GameData()
    {
        _playerPosition = Vector3.zero;
    }
}
