using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int currentLevelIndex;
    public static GameManager Instance;
    private Fading _fader;
    private GameObject _fadeScreenGameObject;
    private bool _inTransition;

    private void Awake()
    {
        Instance = this;
        var fadeScreen = GameObject.FindGameObjectWithTag("FadeScreen");
        if (fadeScreen is not null)
            _fader = fadeScreen.GetComponent<Fading>();
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelAsync());
    }

    public IEnumerator LoadNextLevelAsync(Func<IEnumerator> actionsDuringBlackscreen = null)
    {
        if (_inTransition) yield break;
        if (_fader is null)
            yield return NextLevelWithoutFading(actionsDuringBlackscreen);
        else
        {
            _inTransition = true;
            yield return NextLevelWithFading(actionsDuringBlackscreen);
        }
    }

    private IEnumerator NextLevelWithFading(Func<IEnumerator> actionsDuringBlackscreen)
    {
        yield return _fader.FadeInAsync();
        if (actionsDuringBlackscreen is not null)
            yield return actionsDuringBlackscreen();

        yield return Instance.SwitchSceneAsync();
        yield return new WaitForSeconds(1.0F);
        yield return _fader.FadeOutAsync();
        _inTransition = false;
    }

    private static IEnumerator NextLevelWithoutFading(Func<IEnumerator> actionsDuringBlackscreen)
    {
        if (actionsDuringBlackscreen is not null)
            yield return actionsDuringBlackscreen();
        var operation = Instance.SwitchSceneAsync();
        Debug.Log("Fading skipped. FadeScreen is missing.");
        yield return operation;
    }

    private IEnumerator SwitchSceneAsync()
    {
        currentLevelIndex += 1;
        yield return SceneManager.LoadSceneAsync(currentLevelIndex);
    }
}