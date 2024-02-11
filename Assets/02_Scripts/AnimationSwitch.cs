using UnityEngine;

public class AnimationSwitch : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private PlayerAnimations _animation;
    [SerializeField] private bool _allowJustOnce;
    [SerializeField] private bool _playOnAwake;
    [SerializeField] private bool _playOnTrigger;

    private void Awake()
    {
        if (!ValidateScriptUsage()) return;
        PlayOnAwake();
    }

    private void PlayOnAwake()
    {
        if (!_playOnAwake) return;
        
        var animator = GetComponent<PlayerAnimator>();
        animator.SetAnimation(_animation);
        
        // this component isn't used anymore after play on awake, when trigger is off
        if (!_playOnTrigger) 
            gameObject.SetActive(false);
    }

    private bool ValidateScriptUsage()
    {
        var valid = !_playOnAwake && !_playOnTrigger;
        if (!valid)
            gameObject.SetActive(false);

        return valid;
    }

    // When something collides with a player -> This script is attached to something else
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_playOnTrigger) return;
        if (!other.CompareTag("Player")) return;
        
        var animator = GetComponent<PlayerAnimator>();
        animator.SetAnimation(_animation);
        
        if (_allowJustOnce)
            gameObject.SetActive(false);
    }
}