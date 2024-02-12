using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigid;
    private PlayerAnimator _animator;
    
    [SerializeField] private AudioSource _movementSoundOne;
    [SerializeField] private AudioSource _sprintSoundOne;
    [SerializeField] private AudioSource _landingSound;

    [Header("Movement System")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 2f;
    [SerializeField] private bool _isMoving;
    [SerializeField] private bool _isSprinting;
    [SerializeField] private bool _isFalling;
    
    
    [Header("Wall Detection System")]
    public Transform _wallCheck;
    [Range(0, 90)]
    [SerializeField] private float _wallCollisionAngle = 60;
    [SerializeField] private float _wallScaleX = .5f;
    [SerializeField] private float _wallScaleY = .5f;
    
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

    // Debugging section - It's ok when these vars are not used here - They are just visually for inspector
    // ReSharper disable NotAccessedField.Local
    [Header("Debug")][SerializeField] public bool isJumping;
    [SerializeField] private float _notGroundedSince;
    [FormerlySerializedAs("_groundGhostingDuration")] [SerializeField] private float _groundGhostingTickCount;
    [SerializeField] private float _countJump;
    [SerializeField] private bool _grounded;
    // ReSharper restore NotAccessedField.Local
    
    private int _levelIndex;
    private float _originScale;
    private bool _wasGrounded;
    private bool _collisionWithWall;
    private int _wallCollisionDirection;

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
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (_isBlocked || (!IsGrounded() && IsRunningAgainstWall(horizontalInput)))
        {
            _animator.Walking = false;
            _animator.Sprinting = false;
            _isSprinting = false;
            _isMoving = false;
            return;
        }

        var move = new Vector2(horizontalInput, 0).normalized;
        var velocity = new Vector2(move.x * GetCurrentSpeed(), (_rigid.velocity).y);
        _rigid.velocity = velocity;

        _isMoving = velocity.x != 0;
        if (horizontalInput == 0f)
        {
            _animator.Walking = false;
            _animator.Sprinting = false;
            _isMoving = false;
            return;
        }

        if (_isSprinting && _animator.Walking)
            _animator.Walking = false;
        else if (!_isSprinting && !_animator.Walking)
            _animator.Walking = true;
        _animator.Sprinting = _isSprinting;
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
        _animator.Grounded = grounded;

        if ((!grounded || isJumping) && _notGroundedSince > 0)
            _notGroundedSince++;
        if (_notGroundedSince > _groundGhostingTickCount)
            _notGroundedSince = -1;

        if (grounded && _isFalling)
        {
            _animator.Falling = false;
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
            _animator.Jumping = true;
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
                _animator.Falling = true;
            }
            _rigid.velocity -= _plGravity * (_fallMulti * Time.deltaTime);
        }

        if (!_wasGrounded && grounded)
            _animator.Jumping = false;

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
        return _grounded = Physics2D.OverlapCapsule(_groundCheck.position, new Vector2(_groundScaleX, _groundScaleY), CapsuleDirection2D.Horizontal, 0, _groundLayer);
    }

    private bool IsCollidingWithWall()
    {
        if (_wallCheck is null)
            return false;

        var position = _wallCheck.position;
        var wallcheckPosition = new Vector2(_wallScaleX, _wallScaleY);
        return Physics2D.OverlapCapsule(position, wallcheckPosition, CapsuleDirection2D.Horizontal, _wallCollisionAngle, _groundLayer); 
    }

    private void Flip()
    {
        var scale = transform.localScale;
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

    private bool IsRunningAgainstWall(float direction)
    {
        var clampedDirection = direction > 0 ? 1 : direction == 0 ? 0 : -1;
        if (clampedDirection == 0) return false;
        return IsCollidingWithWall();
    }

    private void InitGetComponent()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _plGravity = new Vector2(0, -Physics2D.gravity.y);
        _animator = GetComponent<PlayerAnimator>();
        var gameManager = FindObjectOfType<GameManager>();
        _levelIndex = gameManager != null ? gameManager._currentLevelIndex : 5;
        _originScale = _rigid.transform.localScale.x;
    }
}