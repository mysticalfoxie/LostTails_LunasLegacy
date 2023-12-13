using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastScene : MonoBehaviour
{
    public StoryScene[] SceneIndex;

    public LastScene()
    {
        SceneIndex = new StoryScene[3];
    }
    public bool IsLastScene(int index)
    {
        return SceneIndex.Length -1 == index;
    }


}
