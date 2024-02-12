using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public static readonly PlayerAnimatorParameters Parameters = new(); 
    
    private Animator _animator;

    private bool _walking;
    private bool _falling;
    private bool _grounded;
    private bool _sprinting;
    private bool _jumping;

    public void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public bool Walking
    {
        get => _walking;
        set => SetWalking(value);
    }

    public bool Falling
    {
        get => _falling;
        set => SetFalling(value);
    }

    public bool Sprinting
    {
        get => _sprinting;
        set => SetSprinting(value);
    }

    public bool Grounded
    {
        get => _grounded;
        set => SetGrounded(value);
    }

    public bool Jumping
    {
        get => _jumping;
        set => SetJumping(value);
    }
    
    private void SetWalking(bool value)
    {
        _walking = value;
        _animator.SetBool(Parameters.Walking, value);
    }

    private void SetSprinting(bool value)
    {
        _sprinting = value;
        _animator.SetBool(Parameters.Sprinting, value);
    }

    private void SetJumping(bool value)
    {
        _jumping = value;
        _animator.SetBool(Parameters.Jumping, value);
    }

    private void SetFalling(bool value)
    {
        _falling = value;
        _animator.SetBool(Parameters.Falling, value);
    }

    private void SetGrounded(bool value)
    {
        _grounded = value;
        _animator.SetBool(Parameters.Grounded, value);
    }

    private void SetIdle()
    {
        Walking = false;
        Sprinting = false;
        Jumping = false;
        Grounded = true;
        Falling = false;
    }

    public void SetAnimation(PlayerAnimations targetAnimation)
    {
        var parameter = Parameters.GetParameter(targetAnimation);
        if (parameter == -1) // -1 => Idle Animation 
            SetIdle();
        else
            _animator.SetBool(parameter, true);
    }
}