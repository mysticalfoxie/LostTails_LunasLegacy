using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BottomBarController : MonoBehaviour
{
    public TextMeshProUGUI barText;
    public TextMeshProUGUI personNameText;
    public float textSpeed = 0.05f;

    public int sentenceIndex = -1;
    private StoryScene currentScene;
    private State state = State.COMPLETED;

    private enum State
    {
        PLAYING, COMPLETED
    }
    public void PlayScene(StoryScene scene)
    {
        currentScene = scene;
        sentenceIndex = -1;
        PlayNextSentence();
    }
    public void PlayNextSentence()
    {
        StartCoroutine(TypeText(currentScene.sentences[++sentenceIndex].text));
        personNameText.text = currentScene.sentences[sentenceIndex].speaker.speakerName;
        personNameText.color = currentScene.sentences[sentenceIndex].speaker.textColor;
    }
    
        
    public bool IsCompleted()
    {
        return state == State.COMPLETED;
    }
    public bool IsLastSentence()
    {
    return sentenceIndex + 1 == currentScene.sentences.Count; 
    }
    public IEnumerator TypeText(string text)
    {
        barText.text = "";
        state = State.PLAYING;
        int wordIndex = 0;

        while (state != State.COMPLETED)
        {
            barText.text += text[wordIndex];
            yield return new WaitForSeconds(textSpeed);
            if(++wordIndex == text.Length)
            {
                state= State.COMPLETED;
                break;
            }
        }
    }
    public void SkipToEnd(float textSpeed)
    {
      new WaitForSeconds(textSpeed +0.05f);
    }
}
