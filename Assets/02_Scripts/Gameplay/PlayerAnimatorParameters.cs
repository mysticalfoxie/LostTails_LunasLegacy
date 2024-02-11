using System;
using UnityEngine;

public class PlayerAnimatorParameters
{
    public readonly int Walking = Animator.StringToHash("IsWalking");
    public readonly int Sprinting = Animator.StringToHash("IsSprinting");
    public readonly int Grounded = Animator.StringToHash("IsGrounded");
    public readonly int Jumping = Animator.StringToHash("IsJumping");
    public readonly int Falling = Animator.StringToHash("IsFalling");
    
    public int GetParameter(PlayerAnimations animation) =>
        animation switch
        {
            PlayerAnimations.Falling => Falling,
            PlayerAnimations.Walking => Walking,
            PlayerAnimations.Sprinting => Sprinting,
            PlayerAnimations.Grounded => Grounded,
            PlayerAnimations.Jumping => Jumping,
            PlayerAnimations.Idle => (int)PlayerAnimations.Idle,
            _ => throw new ArgumentOutOfRangeException(nameof(animation), animation, null)
        };
}