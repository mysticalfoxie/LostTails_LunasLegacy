using UnityEngine;
[System.Serializable]
public class GameData : MonoBehaviour
{
    public Vector3 _playerPosition;

    public GameData()
    {
        _playerPosition = Vector3.zero;
    }
}
