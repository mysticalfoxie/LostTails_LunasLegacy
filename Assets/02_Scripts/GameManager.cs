using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public int currentLevelIndex;
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
