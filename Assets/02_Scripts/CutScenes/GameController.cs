using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public StoryScene currentScene;
    public StoryScene[] allScenes;
    public BottomBarController bottomBar;
    public BackgroundController backgroundController;

    void Start()
    {
        bottomBar.PlayScene(currentScene);
        backgroundController.SetImage(currentScene.background);
        
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
                    var lastScene = allScenes.Last();
                    var isLastScene = lastScene == currentScene;
                    if(allScenes.Last()== currentScene)
                    {
                        SceneManager.LoadScene(1);
                    }
                    currentScene = currentScene.nextScene;
                    bottomBar.PlayScene(currentScene);
                    backgroundController.SetImage(currentScene.background);
                }
                else
                {
                    bottomBar.PlayNextSentence();
                }
            }
        }
    }
}
