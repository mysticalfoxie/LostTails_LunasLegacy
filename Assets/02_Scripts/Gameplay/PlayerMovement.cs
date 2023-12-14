using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D _rigid;
    Animator animator;
    GameManager gameManager;

    [Header("Movement System")][SerializeField] float walkSpeed = 20f;
    [SerializeField] float sprintSpeed = 30f;
    [SerializeField] bool isSprinting;
    [SerializeField] bool isWalking;
    [SerializeField] public bool isBlocked; //Block Movement and Jumping in Dialoque

    [Header("Jump System")][SerializeField] float jumpPower = 30f;
    [SerializeField] float maxJump = 0.4f;
    [SerializeField] float fallMulti;
    [SerializeField] float jumpMulti;
    [SerializeField] float jumpSprint = 2f;

    [Header("Ground System")][SerializeField] float groundScaleX = 7.61f;
    [SerializeField] float groundScaleY = 0.4f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    Vector2 plGravity;

    [Header("Debug")][SerializeField] bool isJumping;
    [SerializeField] float countJump;
    int levelIndex;
    float originScale;

    private static readonly int IsWalkingAnimation = Animator.StringToHash("IsWalking");
    private static readonly int IsSprintingAnimation = Animator.StringToHash("IsSprinting");
    private static readonly int IsGroundedAnimation = Animator.StringToHash("IsGrounded");
    private static readonly int IsJumpingAnimation = Animator.StringToHash("IsJumping");
    private static readonly int IsFallingAnimation = Animator.StringToHash("IsFalling");

    private void Awake()
    {
    }

    void Start()
    {
        initGetComponent();
    }

    void Update()
    {
        HandleInput();
        HandleJump();
    }

    void FixedUpdate()
    {
        Move();
        Flip();
    }

    void HandleInput()
    {
        if (isBlocked) return;
        if (levelIndex == 1 || levelIndex == 3) return;
        Sprint();
    }

    void Move()
    {
        if (isBlocked)
        {
            animator.SetBool(IsWalkingAnimation, false);
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 move = new Vector2(horizontalInput, 0).normalized;
        _rigid.velocity = new Vector2(move.x * GetCurrentSpeed(), _rigid.velocity.y);

        if (horizontalInput == 0f)
        {
            animator.SetBool(IsWalkingAnimation, false);
            animator.SetBool(IsSprintingAnimation, false);
            return;
        }

        if (isSprinting && animator.GetBool(IsWalkingAnimation))
            animator.SetBool(IsWalkingAnimation, false);
        else if (!isSprinting && !animator.GetBool(IsWalkingAnimation))
            animator.SetBool(IsWalkingAnimation, true);
        animator.SetBool(IsSprintingAnimation, isSprinting);
    }

    void Sprint()
    {
        if (Input.GetButton("Sprint") /*&& isGrounded()*/)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    float GetCurrentSpeed()
    {
        return isSprinting ? sprintSpeed : walkSpeed;
    }

    void HandleJump()
    {
        if (levelIndex == 3) return;
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            if (isSprinting == true)
            {
                _rigid.velocity = new Vector2(_rigid.velocity.x+jumpSprint, jumpPower);
            }
            else
            {
                _rigid.velocity = new Vector2(_rigid.velocity.x, jumpPower);
            }
                isJumping = true;
                countJump = 0;
        }

        if (_rigid.velocity.y > 0 && isJumping)
        {
            countJump += Time.deltaTime;
            if (countJump > maxJump) isJumping = false;

            float t = countJump / maxJump;
            float currentJump = jumpMulti;
            if (t > 0.5f)
            {
                if (isSprinting)
                {
                    // currentJump = jumpSprint * (1 - t);
                    currentJump = jumpMulti * (1 - t);
                }
                else
                {
                    currentJump = jumpMulti * (1 - t);

                }
            }

            _rigid.velocity += plGravity * currentJump * Time.deltaTime;
        }

        if (_rigid.velocity.y < 0)
        {
            _rigid.velocity -= plGravity * fallMulti * Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            countJump = 0;

            if (_rigid.velocity.y > 0)
            {
                _rigid.velocity = new Vector2(_rigid.velocity.x, _rigid.velocity.y * 0.6f);
            }
        }
    }

    bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(groundScaleX, groundScaleY), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    void Flip()
    {
        Vector3 scale = this.transform.localScale;
        float ScalingX = originScale;

        if (Input.GetKey(KeyCode.A))
        {
            scale.x = ScalingX;
        } else if (Input.GetKey(KeyCode.D))
        {
            scale.x = -ScalingX;
        }
        this.transform.localScale = scale;
    }

    void initGetComponent()
    {
        _rigid = GetComponent<Rigidbody2D>();
        plGravity = new Vector2(0, -Physics2D.gravity.y);
        animator = GetComponent<Animator>();
        var gameManager = FindObjectOfType<GameManager>();
        levelIndex = gameManager != null ? gameManager.currentLevelIndex : 5;
        originScale = _rigid.transform.localScale.x;
    }
}