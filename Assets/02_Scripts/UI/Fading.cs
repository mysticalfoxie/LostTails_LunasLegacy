using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    [SerializeField] bool fadeIn = false;
    [SerializeField] bool fadeOut = false;
    [SerializeField] float timeToFade;

    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn == true)
        {
            if (CanvasGroup.alpha < 1)
            {
                CanvasGroup.alpha += timeToFade * Time.deltaTime;
                if (CanvasGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }
        if (fadeOut == true)
        {
            if (CanvasGroup.alpha >= 0)
            {
                CanvasGroup.alpha -= timeToFade * Time.deltaTime;
                if (CanvasGroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }
    public void FadeIn()
    {
        Debug.Log("FadeIn wird geladen");
        fadeIn = true;
    }

    public void FadeOut()
    {
        Debug.Log("FadeOut wird geladen");
        fadeOut = true;
    }

    public IEnumerator _ChangeScene()
    {
        Debug.Log("_ChangeScene wird geladen");
        FadeIn();
        yield return new WaitForSeconds(1);
        gameManager.LoadNextLevel();
        FadeOut();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Trigger NextLevel!");
            StartCoroutine(_ChangeScene());
        }
    }
}
