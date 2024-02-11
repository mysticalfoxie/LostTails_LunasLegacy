using System;
using UnityEngine;

public class MovementSwitch : MonoBehaviour
{
    [SerializeField] private SoundSwitchTargetValue _targetValue;
    [SerializeField] private bool _allowJustOnce;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        var movement = other.GetComponent<PlayerMovement>();
        movement.enabled = Convert.ToBoolean(_targetValue);
        
        if (_allowJustOnce)
            gameObject.SetActive(false);
    }
}

public enum SoundSwitchTargetValue
{
    Enabled = 1,
    Disabled = 0
}