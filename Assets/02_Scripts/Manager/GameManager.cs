using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public int currentLevelIndex;
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void LoadNextLevel()
    {
        currentLevelIndex += 1;
        SceneManager.LoadScene(currentLevelIndex);
    }

}
