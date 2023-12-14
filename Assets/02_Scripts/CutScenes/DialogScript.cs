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

    public PlayerMovement playerMovement;

    void Start()
    {
        bottomBar.PlayScene(currentScene);

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
                    playerMovement.isBlocked = false;
                    PressEText.SetActive(false);
                    DialogueCanvas.gameObject.SetActive(false);
                }
                else
                {
                    bottomBar.PlayNextSentence();
                }
            }
        }
    }
}
