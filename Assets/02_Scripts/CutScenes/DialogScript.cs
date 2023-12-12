using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class DialogScript : MonoBehaviour
{
    public StoryScene currentScene;
    public BottomBarController bottomBar;
    

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
                    PlayerMovement isBlocked = GetComponent<PlayerMovement>();
                    isBlocked.isBlocked = false;
                    
                }
                else
                {
                    bottomBar.PlayNextSentence();
                }
            }
        }
    }
}
