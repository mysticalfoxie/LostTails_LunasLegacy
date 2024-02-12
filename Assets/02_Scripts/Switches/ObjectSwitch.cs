using System;
using UnityEngine;

public class ObjectSwitch : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private ObjectSwitchTargetValue _targetValue;
    [SerializeField] private bool _allowJustOnce;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var state = Convert.ToBoolean(_targetValue);
        _target.SetActive(state);

        enabled = !_allowJustOnce && enabled;
    }
}

public enum ObjectSwitchTargetValue
{
    Enabled = 1,
    Disabled = 0
}