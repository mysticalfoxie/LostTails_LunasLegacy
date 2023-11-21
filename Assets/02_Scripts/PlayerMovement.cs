using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D _rigid;
    Collider2D c_Collider;
    SpriteRenderer mySprite;

    [Header("Movement System")]
    [SerializeField] float walkSpeed = 20f;
    [SerializeField] float sprintSpeed = 30f;
    [SerializeField] bool isSprinting;

    [Header("Jump System")]
    [SerializeField] float jumpPower = 30f;
    [SerializeField] float maxJump = 0.4f;
    [SerializeField] float fallMulti;
    [SerializeField] float jumpMulti;

    [Header("Config for Ground Check")]
    [SerializeField] float groundScaleX = 7.61f;
    [SerializeField] float groundScaleY = 0.4f;

    public Transform groundCheck;
    public LayerMask groundLayer;
    Vector2 plGravity;

    [Header("Debug")]
    [SerializeField] bool isJumping;
    [SerializeField] float countJump;

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        c_Collider = GetComponent<Collider2D>();
        mySprite = GetComponent<SpriteRenderer>();
        plGravity = new Vector2(0, -Physics2D.gravity.y);
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
        Sprint();
    }

    void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 move = new Vector2(horizontalInput, 0).normalized;
        _rigid.velocity = new Vector2(move.x * GetCurrentSpeed(), _rigid.velocity.y);
    }

    void Sprint()
    {
        if (Input.GetButton("Sprint"))
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
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, jumpPower);
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
                currentJump = jumpMulti * (1 - t);
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
        Vector2 scale = transform.localScale;
        if (_rigid.velocity.x > 0f)
        {
            mySprite.flipX = true;
        }
        else if (_rigid.velocity.x < 0f)
        {
            mySprite.flipX = false;
        }
    }
}
