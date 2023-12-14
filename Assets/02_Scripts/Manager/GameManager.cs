using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class GameManager : MonoBehaviour
{
    public int currentLevelIndex;
    public static GameManager Instance;
    Fading fading;


    public GameManager()
    {

    }
    
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        fading = FindAnyObjectByType<Fading>();
        fading?.FadeOut();
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
