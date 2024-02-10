using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerNextLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("NextLevel")) return;
        DisablePlayerMovement();
        GameManager.LoadNextLevel();
    }

    private static void DisablePlayerMovement()
    {
        var scene = SceneManager.GetActiveScene();
        var rootObjects = scene.GetRootGameObjects();
        var player = rootObjects.FirstOrDefault(x => x.CompareTag("Player"));
        if (player == null) return;
        var movement = player.GetComponent<PlayerMovement>();
        if (movement == null) return;
        movement.enabled = false;
    }
}