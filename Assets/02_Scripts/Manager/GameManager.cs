using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    private Fading _fader;
    private GameObject _fadeScreenGameObject;
    private bool _inTransition;
    
    [Header("Respawning")]
    [Range(0F, 5.0F)] [SerializeField] private float _delayBetweenFading = 20;
    [Range(0.1F, 5.0F)] [SerializeField] private float _fadingInSpeedModifier = 2.5F;
    [Range(0.1F, 5.0F)] [SerializeField] private float _fadingOutSpeedModifier = 2.5F;
    
    [Header("Debugging")]
    [SerializeField] internal int _currentLevelIndex;

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

        SceneManager.sceneLoaded += (scene, _) => _currentLevelIndex = scene.buildIndex;

        DontDestroyOnLoad(Instance);
    }

    public void Start()
    {
        if (DataPersistenceManager.Instance is null) return;
        _currentLevelIndex = DataPersistenceManager.Instance
            .GetLevelIndex()
            .GetValueOrDefault();
    }

    public void Update()
    {
        if (!Debug.isDebugBuild) return;
        if (Input.GetKeyDown(KeyCode.F10) && Input.GetKey(KeyCode.LeftControl))
            LoadLevel(11);
        if (Input.GetKeyDown(KeyCode.F12) && Input.GetKey(KeyCode.LeftControl))
            LoadNextLevel();
        if (Input.GetKeyDown(KeyCode.F11) && Input.GetKey(KeyCode.LeftControl))
            LoadLevel(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public static void RespawnPlayer(RespawnMethod method, GameObject respawn = null)
    {
        if (method == RespawnMethod.PositionUpdate && respawn is null)
            throw new ArgumentNullException(nameof(respawn));

        var player = GetPlayer();
        if (player is null)
        {
            Debug.LogWarning("The scene does not include a GameObject with Tag \"Player\". Respawn aborted.");
            return;
        }
        
        Instance.RespawnPlayerInternal(method, respawn, player);
    }

    private void RespawnPlayerInternal(RespawnMethod method, GameObject respawn, GameObject player)
    {

        switch (method)
        {
            case RespawnMethod.PositionUpdate:
                StartCoroutine(RespawnAtGameObject(respawn, player));
                break;
            case RespawnMethod.SceneReload:
                StartCoroutine(RespawnBySceneReload());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(method), method, null);
        }
    }
    
    private IEnumerator RespawnAtGameObject(GameObject respawn, GameObject player)
    {
        if (_fader is null)
        {
            player.transform.position = respawn.transform.position;
            yield break;
        }

        var movement = player.GetComponent<PlayerMovement>(); 
        movement._isDying = true;
        yield return _fader.FadeInAsync(_fadingInSpeedModifier);
        movement._isDying = false;
        player.transform.position = respawn.transform.position;
        movement._isSpawning = true;
        yield return Instance._fader.FadeOutAsync(_fadingOutSpeedModifier);
        movement._isSpawning = false;
    }

    private IEnumerator RespawnBySceneReload()
    {
        yield return LoadLevelAsync(_currentLevelIndex, _delayBetweenFading, _fadingInSpeedModifier, _fadingOutSpeedModifier);
        // no need to enable it again. it's already enabled after the reload ;)
    }

    private static GameObject GetPlayer()
    {
        var scene = SceneManager.GetActiveScene();
        var objects = scene.GetRootGameObjects();
        return FindGameObjectRecursively(objects, x => x.CompareTag("Player"));
    }

    // This method searches first on root hierarchy and when no match is found the whole hierarchy -> saves a lot of performance
    private static GameObject FindGameObjectRecursively(GameObject[] gameObjects, Func<GameObject, bool> predicate)
    {
        var match = gameObjects.FirstOrDefault(predicate);
        if (match is not null) return match;
        return gameObjects.FirstOrDefault(x => FindGameObjectRecursively(GetChildren(x).ToArray(), predicate));
    }

    private static IEnumerable<GameObject> GetChildren(GameObject root)
    {
        for (var i = 0; i < root.transform.childCount; i++)
            yield return root.transform.GetChild(i).gameObject;
    }

    public static void LoadNextLevel(float delayBetweenFadings = 1.0F, float fadeInSpeedMod = 1.0F, float fadeOutSpeedMod = 1.0F, Func<IEnumerator> actionsDuringBlackscreen = null)
    {
        var activeScene = SceneManager.GetActiveScene();
        Instance._currentLevelIndex = activeScene.buildIndex + 1;
        LoadLevel(Instance._currentLevelIndex, delayBetweenFadings, fadeInSpeedMod, fadeOutSpeedMod, actionsDuringBlackscreen);
    }

    public static IEnumerator LoadNextLevelAsync(float delayBetweenFadings = 1.0F, float fadeInSpeedMod = 1.0F, float fadeOutSpeedMod = 1.0F, Func<IEnumerator> actionsDuringBlackscreen = null)
    {
        var activeScene = SceneManager.GetActiveScene();
        Instance._currentLevelIndex = activeScene.buildIndex + 1;
        return LoadLevelAsync(Instance._currentLevelIndex, delayBetweenFadings, fadeInSpeedMod, fadeOutSpeedMod, actionsDuringBlackscreen);
    }

    private static IEnumerator LoadLevelWithFading(int levelIndex, float delayBetweenFadings, float fadeInSpeedMod, float fadeOutSpeedMod, Func<IEnumerator> actionsDuringBlackscreen)
    {
        var movement = GetPlayer()?.GetComponent<PlayerMovement>(); 
        if (movement is not null) movement._isDying = true;
        
        yield return Instance._fader.FadeInAsync(fadeInSpeedMod);
        if (actionsDuringBlackscreen is not null)
            yield return actionsDuringBlackscreen();
        
        yield return SceneManager.LoadSceneAsync(levelIndex);
        
        movement = GetPlayer()?.GetComponent<PlayerMovement>(); 
        if (movement is not null) movement._isSpawning = true;
        
        yield return new WaitForSeconds(delayBetweenFadings);
        
        yield return Instance._fader.FadeOutAsync(fadeOutSpeedMod);

        Instance._inTransition = false;
        if (movement is not null) movement._isSpawning = false;
    }

    private static IEnumerator LoadLevelWithoutFading(int levelIndex, Func<IEnumerator> actionsDuringBlackscreen)
    {
        if (actionsDuringBlackscreen is not null)
            yield return actionsDuringBlackscreen();
        
        Debug.Log("Fading skipped. FadeScreen is missing.");
        yield return SceneManager.LoadSceneAsync(levelIndex);
    }

    public static IEnumerator LoadLevelAsync(int levelIndex, float delayBetweenFadings = 1.0F, float fadeInSpeedMod = 1.0F, float fadeOutSpeedMod = 1.0F, Func<IEnumerator> actionsDuringBlackscreen = null)
    {
        if (Instance._inTransition) yield break;
            
        if (Instance._fader is null)
            yield return LoadLevelWithoutFading(levelIndex, actionsDuringBlackscreen);
        else
        {
            Instance._inTransition = true;
            yield return LoadLevelWithFading(levelIndex, delayBetweenFadings, fadeInSpeedMod, fadeOutSpeedMod, actionsDuringBlackscreen);
        }
        
        if (DataPersistenceManager.Instance is not null)
            DataPersistenceManager.Instance.UpdateLevelIndex(levelIndex);
    }
    
    public static void LoadLevel(int levelIndex, float delayBetweenFadings = 1.0F, float fadeInSpeedMod = 1.0F, float fadeOutSpeedMod = 1.0F, Func<IEnumerator> actionsDuringBlackscreen = null)
    {
        var task = LoadLevelAsync(levelIndex, delayBetweenFadings, fadeInSpeedMod, fadeOutSpeedMod, actionsDuringBlackscreen);
        Instance.StartCoroutine(task);
    }
}

public enum RespawnMethod
{
    PositionUpdate,
    SceneReload 
}