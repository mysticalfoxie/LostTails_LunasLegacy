using System;
using UnityEngine;

public class MovementSwitch : MonoBehaviour
{
    [SerializeField] private MovementSwitchTargetValue _targetValue;
    [SerializeField] private bool _allowJustOnce;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        var movement = other.GetComponent<PlayerMovement>();
        movement._isBlocked = !Convert.ToBoolean(_targetValue);

        enabled = !_allowJustOnce && enabled;
    }
}

public enum MovementSwitchTargetValue
{
    Enabled = 1,
    Disabled = 0
}