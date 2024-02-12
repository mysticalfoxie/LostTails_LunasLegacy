using UnityEngine;

public class WallCheckSwitch : MonoBehaviour
{
    private Transform _wallCheck;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var movement = other.GetComponent<PlayerMovement>();
        // ReSharper disable once MergeSequentialChecks => not with unity ;/
        if (movement is null || movement._wallCheck is null) return;
        
        _wallCheck = movement._wallCheck;
        movement._wallCheck = null;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var movement = other.GetComponent<PlayerMovement>();
        
        // ReSharper disable once MergeSequentialChecks => not with unity ;/
        if (movement is null) return;
        
        movement._wallCheck = _wallCheck;
        _wallCheck = null;
    }
}