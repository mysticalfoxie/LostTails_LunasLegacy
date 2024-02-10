using UnityEngine;

public class TriggerNextLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        DisablePlayerMovement(other);
        GameManager.LoadNextLevel();
    }

    private static void DisablePlayerMovement(Collider2D player)
    {
        var movement = player.GetComponent<PlayerMovement>();
        if (movement == null) return;
        movement.enabled = false;
    }
}