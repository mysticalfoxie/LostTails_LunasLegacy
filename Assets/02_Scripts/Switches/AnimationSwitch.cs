using UnityEngine;

public class AnimationSwitch : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private PlayerAnimations _animation;
    [SerializeField] private bool _allowJustOnce;
    [SerializeField] private bool _playOnStart;
    [SerializeField] private bool _playOnTrigger;

    private void Awake()
    {
        if (!_playOnStart && !_playOnTrigger)
            enabled = false;
    }

    public void Start()
    {
        PlayOnAwake();
    }
    
    private void PlayOnAwake()
    {
        if (!_playOnStart) return;
        if (!enabled) return;
        
        var animator = GetComponent<PlayerAnimator>();
        animator.SetAnimation(_animation);
        
        // this component isn't used anymore after play on awake, when trigger is off
        if (!_playOnTrigger) 
            enabled = false;
    }

    // When something collides with a player -> This script is attached to something else
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_playOnTrigger) return;
        if (!enabled) return;
        if (!other.CompareTag("Player")) return;
        
        var animator = other.GetComponent<PlayerAnimator>();
        animator.SetAnimation(_animation);

        enabled = !_allowJustOnce && enabled;
    }
}