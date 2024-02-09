using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigid;
    private Animator _animator;
    [SerializeField] private AudioSource _movementSoundOne;
    [SerializeField] private AudioSource _sprintSoundOne;
    [SerializeField] private AudioSource _landingSound;

    [Header("Movement System")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 2f;
    [SerializeField] private bool _isMoving;
    [SerializeField] private bool _isSprinting;
    [SerializeField] private bool _isFalling;
    [SerializeField] public bool _isBlocked; //Block Movement and Jumping in Dialogue

    [Header("Jump System")]
    [SerializeField] private float _jumpPower = 10f;
    [SerializeField] private float _maxJump = 0.4f;
    [SerializeField] private float _fallMulti;
    [SerializeField] private float _jumpMulti;
    [SerializeField] private float _jumpSprint = 2f;

    [Header("Ground System")]
    [SerializeField] private float _groundScaleX = 6.41f;
    [SerializeField] private float _groundScaleY = 0.2f;
    public Transform _groundCheck;
    public LayerMask _groundLayer;
    private Vector2 _plGravity;

    [Header("Debug")][SerializeField] public bool isJumping;
    [SerializeField] private float _notGroundedSince;
    [FormerlySerializedAs("_groundGhostingDuration")] [SerializeField] private float _groundGhostingTickCount;
    [SerializeField] private float _countJump;
    private int _levelIndex;
    private float _originScale;
    private bool _wasGrounded;

    private static readonly int IsWalkingAnimation = Animator.StringToHash("IsWalking");
    private static readonly int IsSprintingAnimation = Animator.StringToHash("IsSprinting");
    private static readonly int IsGroundedAnimation = Animator.StringToHash("IsGrounded");
    private static readonly int IsJumpingAnimation = Animator.StringToHash("IsJumping");
    private static readonly int IsFallingAnimation = Animator.StringToHash("IsFalling");

    private void Start()
    {
        InitGetComponent();
    }

    private void Update()
    {
        HandleInput();
        HandleJump();
    }

    private void FixedUpdate()
    {
        Move();
        Flip();
        HandleSound();
    }

    private void HandleInput()
    {
        var grounded = IsGrounded();
        if (_isBlocked) return;
        if (_levelIndex == 2) return;
        if (!grounded) return;
        Sprint();
    }

    private void Move()
    {
        if (_isBlocked)
        {
            _animator.SetBool(IsWalkingAnimation, false);
            _isMoving = false;
            return;
        }

        var horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 move = new Vector2(horizontalInput, 0).normalized;
        var velocity = new Vector2(move.x * GetCurrentSpeed(), (_rigid.velocity).y);
        _rigid.velocity = velocity;

        _isMoving = velocity.x != 0;
        if (horizontalInput == 0f)
        {
            _animator.SetBool(IsWalkingAnimation, false);
            _animator.SetBool(IsSprintingAnimation, false);
            _isMoving = false;
            return;
        }

        if (_isSprinting && _animator.GetBool(IsWalkingAnimation))
            _animator.SetBool(IsWalkingAnimation, false);
        else if (!_isSprinting && !_animator.GetBool(IsWalkingAnimation))
            _animator.SetBool(IsWalkingAnimation, true);
        _animator.SetBool(IsSprintingAnimation, _isSprinting);
    }

    private void Sprint()
    {
        _isSprinting = Input.GetButton("Sprint");
    }

    private float GetCurrentSpeed()
    {
        return _isSprinting ? _sprintSpeed : _walkSpeed;
    }

    private void HandleJump()
    {
        var grounded = IsGrounded();
        _animator.SetBool(IsGroundedAnimation, grounded);

        if ((!grounded || isJumping) && _notGroundedSince > 0)
            _notGroundedSince++;
        if (_notGroundedSince > _groundGhostingTickCount)
            _notGroundedSince = -1;

        if (grounded && _isFalling)
        {
            _animator.SetBool(IsFallingAnimation, false);
            _isFalling = false;
            if (!_landingSound.isPlaying)
            {
                _landingSound.Play();
            }
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            _rigid.velocity = _isSprinting
                ? new Vector2(_rigid.velocity.x + _jumpSprint, _jumpPower)
                : new Vector2(_rigid.velocity.x, _jumpPower);
            _animator.SetBool(IsJumpingAnimation, true);
            _notGroundedSince = 1;
            isJumping = true;
            _countJump = 0;
        }

        if (_rigid.velocity.y > 0 && isJumping)
        {
            _countJump += Time.deltaTime;
            if (_countJump > _maxJump)
                isJumping = false;

            var t = _countJump / _maxJump;
            var currentJump = _jumpMulti;
            if (t > 0.5f)
            {
                if (_isSprinting)
                {
                    currentJump = _jumpMulti * (1 - t);
                }
                else
                {
                    currentJump = _jumpMulti * (1 - t);
                }
            }

            _rigid.velocity += _plGravity * (currentJump * Time.deltaTime);
        }

        if (_rigid.velocity.y < 0)
        {
            if (!grounded) { 
                _isFalling = true;
                _animator.SetBool(IsFallingAnimation, true);
            }
            _rigid.velocity -= _plGravity * (_fallMulti * Time.deltaTime);
        }

        if (!_wasGrounded && grounded)
            _animator.SetBool(IsJumpingAnimation, false);

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            _countJump = 0;

            if (_rigid.velocity.y > 0)
            {
                var velocity = _rigid.velocity;
                velocity = new Vector2(velocity.x, velocity.y * 0.6f);
                _rigid.velocity = velocity;
            }
        }
        
        _wasGrounded = grounded;
    }

    private bool IsGrounded()
    {
        if (_notGroundedSince > 0 && (isJumping || _notGroundedSince < _groundGhostingTickCount))
            return false;
        return Physics2D.OverlapCapsule(_groundCheck.position, new Vector2(_groundScaleX, _groundScaleY), CapsuleDirection2D.Horizontal, 0, _groundLayer);
    }

    private void Flip()
    {
        Vector3 scale = this.transform.localScale;
        var scalingX = _originScale;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            scale.x = scalingX;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            scale.x = -scalingX;
        this.transform.localScale = scale;
    }

    private void HandleSound()
    {
        if (!_isMoving && _sprintSoundOne.isPlaying)
        {
            _sprintSoundOne.Stop();
        }
        if (!_isMoving && _movementSoundOne.isPlaying)
        {
            _movementSoundOne.Stop();
        }
        if(_isMoving && !isJumping && IsGrounded() && _isSprinting && !_sprintSoundOne.isPlaying)
        {
            _sprintSoundOne.Play();
        }
        if(_isMoving && !isJumping && IsGrounded() && !_isSprinting && !_movementSoundOne.isPlaying)
        {
            _movementSoundOne.Play();
        }
    }

    private void InitGetComponent()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _plGravity = new Vector2(0, -Physics2D.gravity.y);
        _animator = GetComponent<Animator>();
        var gameManager = FindObjectOfType<GameManager>();
        _levelIndex = gameManager != null ? gameManager._currentLevelIndex : 5;
        _originScale = _rigid.transform.localScale.x;
    }
}