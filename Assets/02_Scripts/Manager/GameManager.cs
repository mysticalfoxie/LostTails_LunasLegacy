using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int _currentLevelIndex;
    public static GameManager Instance;
    private Fading _fader;
    private GameObject _fadeScreenGameObject;
    private bool _inTransition;

    private void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        var fadeScreen = GameObject.FindGameObjectWithTag("FadeScreen");
        if (fadeScreen is not null)
            Instance._fader = fadeScreen.GetComponent<Fading>();

        DontDestroyOnLoad(Instance);
    }

    public void Start()
    {
        _currentLevelIndex = DataPersistenceManager.Instance
            .GetLevelIndex()
            .GetValueOrDefault();
    }

    public static void LoadNextLevel()
    {
        var activeScene = SceneManager.GetActiveScene();
        Instance._currentLevelIndex = activeScene.buildIndex + 1;
        LoadLevel(Instance._currentLevelIndex);
    }

    public static IEnumerator LoadNextLevelAsync(Func<IEnumerator> actionsDuringBlackscreen = null)
    {
        var activeScene = SceneManager.GetActiveScene();
        Instance._currentLevelIndex = activeScene.buildIndex + 1;
        return LoadLevelAsync(Instance._currentLevelIndex, actionsDuringBlackscreen);
    }

    private static IEnumerator LoadLevelWithFading(int levelIndex, Func<IEnumerator> actionsDuringBlackscreen)
    {
        yield return Instance._fader.FadeInAsync();
        if (actionsDuringBlackscreen is not null)
            yield return actionsDuringBlackscreen();
        yield return SceneManager.LoadSceneAsync(levelIndex);
        yield return new WaitForSeconds(1.0F);
        yield return Instance._fader.FadeOutAsync();

        Instance._inTransition = false;
    }

    private static IEnumerator LoadLevelWithoutFading(int levelIndex, Func<IEnumerator> actionsDuringBlackscreen)
    {
        if (actionsDuringBlackscreen is not null)
            yield return actionsDuringBlackscreen();
        
        Debug.Log("Fading skipped. FadeScreen is missing.");
        yield return SceneManager.LoadSceneAsync(levelIndex);
    }

    public static IEnumerator LoadLevelAsync(int levelIndex, Func<IEnumerator> actionsDuringBlackscreen = null)
    {
        if (Instance._inTransition) yield break;
            
        if (Instance._fader is null)
            yield return LoadLevelWithoutFading(levelIndex, actionsDuringBlackscreen);
        else
        {
            Instance._inTransition = true;
            yield return LoadLevelWithFading(levelIndex, actionsDuringBlackscreen);
        }
        
        DataPersistenceManager.Instance.UpdateLevelIndex(levelIndex);
    }
    
    public static void LoadLevel(int levelIndex, Func<IEnumerator> actionsDuringBlackscreen = null)
    {
        var task = LoadLevelAsync(levelIndex, actionsDuringBlackscreen);
        Instance.StartCoroutine(task);
    }
}