using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;


public class DialogScript : MonoBehaviour
{
    public StoryScene currentScene;
    public BottomBarController bottomBar;
    public GameObject DialogueCanvas;
    public GameObject PressEText;

    void Start()
    {
        bottomBar.PlayScene(currentScene);
        PlayerMovement moveScript = GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (bottomBar.IsCompleted())
            {
                if (bottomBar.IsLastSentence())
                {
                    PressEText.SetActive(false);
                    DialogueCanvas.gameObject.SetActive(false);
                    PlayerMovement moveScript = GetComponent<PlayerMovement>();
                    moveScript.isBlocked = false;
                    
                }
                else
                {
                    bottomBar.PlayNextSentence();
                }
            }
        }
    }
}
