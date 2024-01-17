using UnityEngine;

public class TriggerNextLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("NextLevel")) return;
        GameManager.Instance.LoadNextLevel();
    }
}