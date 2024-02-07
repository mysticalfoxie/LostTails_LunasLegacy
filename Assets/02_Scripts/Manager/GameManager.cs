using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currentLevelIndex;
    public int savedLevelIndex;
    public static GameManager Instance;
    private Fading _fader;
    private GameObject _fadeScreenGameObject;
    private bool _inTransition;
    
    [SerializeField] private float _progress;

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

    public static void LoadNextLevel()
    {
        Instance.StartCoroutine(LoadNextLevelAsync());
    }

    public static IEnumerator LoadNextLevelAsync(Func<IEnumerator> actionsDuringBlackscreen = null)
    {
        if (Instance._inTransition) yield break;
        if (Instance._fader is null)
            yield return NextLevelWithoutFading(actionsDuringBlackscreen);
        else
        {
            Instance._inTransition = true;
            yield return NextLevelWithFading(actionsDuringBlackscreen);
        }
    }

    private static IEnumerator NextLevelWithFading(Func<IEnumerator> actionsDuringBlackscreen)
    {
        yield return Instance._fader.FadeInAsync();
        if (actionsDuringBlackscreen is not null)
            yield return actionsDuringBlackscreen();
        yield return SwitchSceneAsync();
        yield return new WaitForSeconds(1.0F);
        yield return Instance._fader.FadeOutAsync();
        
        Instance._inTransition = false;
    }

    private static IEnumerator NextLevelWithoutFading(Func<IEnumerator> actionsDuringBlackscreen)
    {
        if (actionsDuringBlackscreen is not null)
            yield return actionsDuringBlackscreen();
        Debug.Log("Fading skipped. FadeScreen is missing.");
        yield return SwitchSceneAsync();
    }

    private static IEnumerator SwitchSceneAsync()
    {
        Instance.currentLevelIndex += 1;
        yield return SceneManager.LoadSceneAsync(Instance.currentLevelIndex);
    }
}