using UnityEngine;

public class TriggerNextLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter");
        if (!other.gameObject.CompareTag("NextLevel")) return;
        Debug.Log("CompareTag(NextLevel)");
        GameManager.Instance.LoadNextLevel();
        Debug.Log("LoadNextLevel");
    }
}