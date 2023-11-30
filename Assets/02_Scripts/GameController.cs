using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public StoryScene currentScene;
    public BottomBarController bottomBar;
    // Start is called before the first frame update
    void Start()
    {
        bottomBar.PlayScene(currentScene);  
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Space))
        {
            if(bottomBar.IsCompleted())
            { 
                if(bottomBar.IsLastSentence())
                {
                    currentScene = currentScene.nextScene;
                    bottomBar.PlayScene(currentScene);
                }
                bottomBar.PlayNextSentence();

            }
        }
    }
}
