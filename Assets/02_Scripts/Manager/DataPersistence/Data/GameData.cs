using UnityEngine;
[System.Serializable]
public class GameData : MonoBehaviour
{
    public Vector3 _playerPosition;
    GameManager _gameManager;

    public GameData()
    {
        _playerPosition = Vector3.zero;
        _gameManager.savedLevelIndex = PlayerPrefs.GetInt("SavedLevel");
    }
}
