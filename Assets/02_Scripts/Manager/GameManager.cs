using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class GameManager : MonoBehaviour
{
    public int currentLevelIndex;
    public static GameManager Instance;


    public GameManager()
    {
        if (FindObjectsOfType<GameManager>().Length > 0)
            throw new Exception("JUSTIN/TONI!!! STOP IT!.... Es können keine 2 GameManager in einer Szene sein!");
    }
    
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
