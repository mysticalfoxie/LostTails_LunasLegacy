using System.Runtime.Serialization;
using UnityEngine;

public class SoundSwitch : MonoBehaviour
{
    [SerializeField] [OptionalField] private AudioSource _target;
    [SerializeField] private bool _allowJustOnce;

    private void Awake()
    {
        _target ??= GetComponent<AudioSource>();
        if (_target is null)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        _target.Play();
        
        if (_allowJustOnce)
            gameObject.SetActive(false);
    }
}