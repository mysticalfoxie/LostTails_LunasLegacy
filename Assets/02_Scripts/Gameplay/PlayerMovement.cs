using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D _rigid;
    Animator animator;
    [SerializeField] AudioSource movementSoundOne;
    [SerializeField] AudioSource movementSoundTwo;
    [SerializeField] AudioSource sprintSoundOne;
    [SerializeField] AudioSource sprintSoundTwo;
    [SerializeField] AudioSource jumpSound;
    [SerializeField] AudioSource landingSound;

    [Header("Movement System")][SerializeField] float walkSpeed = 20f;
    [SerializeField] float sprintSpeed = 30f;
    [SerializeField] bool isMoving;
    [SerializeField] bool isSprinting;
    [SerializeField] bool isWalking;
    [SerializeField] bool isFalling;
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
    [SerializeField] private float _notGroundedSince;
    [FormerlySerializedAs("_groundGhostingDuration")] [SerializeField] private float _groundGhostingTickCount;
    [SerializeField] float countJump;
    int levelIndex;
    float originScale;
    private bool _wasGrounded;


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
        HandleSound();
    }

    void HandleInput()
    {
        if (isBlocked) return;
        if (levelIndex == 2) return;
        Sprint();
    }

    void Move()
    {
        if (isBlocked)
        {
            animator.SetBool(IsWalkingAnimation, false);
            isMoving = false;
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 move = new Vector2(horizontalInput, 0).normalized;
        _rigid.velocity = new Vector2(move.x * GetCurrentSpeed(), _rigid.velocity.y);
     //   isMoving = true;

        if ( _rigid.velocity.x != 0)
        {
            isMoving = true;
        } else
        {
            isMoving = false;
        }
        if (horizontalInput == 0f)
        {
            animator.SetBool(IsWalkingAnimation, false);
            animator.SetBool(IsSprintingAnimation, false);
            isMoving = false;
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
        var grounded = isGrounded();
        animator.SetBool(IsGroundedAnimation, grounded);

        if (!grounded || isJumping)
            _notGroundedSince++;
        if (_notGroundedSince > _groundGhostingTickCount)
            _notGroundedSince = 0;

        if (grounded && isFalling)
        {
            animator.SetBool(IsFallingAnimation, false);
            isFalling = false;
            if (!landingSound.isPlaying)
            {
                landingSound.Play();
            }
        }
        
        if (Input.GetButtonDown("Jump") && grounded)
        {
            if (isSprinting == true)
            {
                _rigid.velocity = new Vector2(_rigid.velocity.x+jumpSprint, jumpPower);
            }
            else
            {
                _rigid.velocity = new Vector2(_rigid.velocity.x, jumpPower);
            }
            animator.SetBool(IsJumpingAnimation, true);
            _notGroundedSince = 1;
            isJumping = true;
            countJump = 0;
          /*  if (!jumpSound.isPlaying) //WIP Future Sound for Jump!
            {
                jumpSound.Play();
            }*/
        }

        if (_rigid.velocity.y > 0 && isJumping)
        {
            countJump += Time.deltaTime;
            if (countJump > maxJump)
                isJumping = false;

            float t = countJump / maxJump;
            float currentJump = jumpMulti;
            if (t > 0.5f)
            {
                if (isSprinting)
                {
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
            if (!grounded) { 
                isFalling = true;
                animator.SetBool(IsFallingAnimation, true);
            }
            _rigid.velocity -= plGravity * fallMulti * Time.deltaTime;
        }

        if (!_wasGrounded && grounded) // Wenn der Player im letzten Tick noch nicht grounded war es aber jetzt ist hört der Sprung auf.
            animator.SetBool(IsJumpingAnimation, false);

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            countJump = 0;

            if (_rigid.velocity.y > 0)
            {
                _rigid.velocity = new Vector2(_rigid.velocity.x, _rigid.velocity.y * 0.6f);
            }
        }
        
        _wasGrounded = grounded;
    }

    bool isGrounded()
    {
        // Das ist für die ersten paar Momente wo der Spieler gerade erst abgesprungen ist.
        // Der Collider von dem Ground Check ist in dem Moment immer noch in dem Boden drin, auch wenn er gerade am absprung ist.
        // Hier würd quasie für "_groundGhostingTickCount" ticks ignoriert dass er überlappt.
        if (_notGroundedSince > 0 && _notGroundedSince < _groundGhostingTickCount)
            return false;

        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(groundScaleX, groundScaleY), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    void Flip()
    {
        Vector3 scale = this.transform.localScale;
        float ScalingX = originScale;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            scale.x = ScalingX;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            scale.x = -ScalingX;
        }
        this.transform.localScale = scale;
    }
    void HandleSound()
    {
        if (!isMoving && sprintSoundOne.isPlaying)
        {
            sprintSoundOne.Stop();
        }
        if (!isMoving && movementSoundOne.isPlaying)
        {
            movementSoundOne.Stop();
        }
        if(isMoving && !isJumping && isGrounded() && isSprinting && !sprintSoundOne.isPlaying)
        {
            sprintSoundOne.Play();
        }
        if(isMoving && !isJumping && isGrounded() && !isSprinting && !movementSoundOne.isPlaying)
        {
            movementSoundOne.Play();
        }
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