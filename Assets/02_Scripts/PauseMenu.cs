using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    bool Paused = false;

    private void Awake()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (Paused == false)
            {
                Pause();
                /*Cursor.visible = false;
                Time.timeScale = 1.0f;
                pauseMenu.SetActive(false);
                Paused = false;*/

            } else
            {
                Resume();
                /*Time.timeScale = 0.0f;
                Cursor.visible = true;
                pauseMenu.SetActive(true);
                Paused = true;*/
            }
        }
    }

    public void Pause()
    {
        Cursor.visible = true;
        Time.timeScale = 0.0f;
        pauseMenu.SetActive(true);
        Paused = true;
    }
    public void Resume()
    {
        Cursor.visible = false;
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
        Paused = false;
    }

    public void Home()
    {
        if (Time.timeScale == 0) Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
