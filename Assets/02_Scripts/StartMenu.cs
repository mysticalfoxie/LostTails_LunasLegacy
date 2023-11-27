using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    //To-Do: How To Play?, Options Button, Menu Sound?
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Credits()
    {

    }

    public void Options() //To-Do: Sound Slider!
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
