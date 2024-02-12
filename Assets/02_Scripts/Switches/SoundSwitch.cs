using UnityEngine;

public class SoundSwitch : MonoBehaviour
{
    [SerializeField] private AudioSource _target;
    [SerializeField] private bool _allowJustOnce;

    private void Awake()
    {
        _target ??= GetComponent<AudioSource>();
        if (_target is null)
            enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!enabled) return;
        _target.Play();
        
        enabled = !_allowJustOnce && enabled;
    }
}
