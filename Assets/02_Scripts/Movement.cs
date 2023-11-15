using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float sprintSpeed = 2f;
    [SerializeField] float crouchSpeed = 0.5f;

    [SerializeField] float jumpHeight = 250f;
    [SerializeField] float originalJump = 250f;
    [SerializeField] float crouchJump = 175f;

    [SerializeField] float originalWidth = 1f;
    [SerializeField] float originalHeight = 1000f;
    [SerializeField] float crouchHeight = 0.5f;

    [SerializeField] bool isJumping = false;
    [SerializeField] bool isGrounded = true;
    [SerializeField] bool isSprinting = false;
    [SerializeField] bool isCrouching = false;

    [Range(0, 1f)][SerializeField] float smoothing = 0.5f;

    private Vector3 velocity0 = Vector3.zero;

    [SerializeField] Rigidbody2D _rigid;
    LayerMask Character;

    [SerializeField] BoxCollider2D c_Collider;

    void Start()
    {
    
    }
    void Awake()
    {

    }

    void Update()
    {
        Move();
        Flip();
        Jump();
    }

    void Move()
    {
        var horizontal = Input.GetAxis("Horizontal");
        Sprint();
        Crouch();
        velocity0 = _rigid.velocity;
        var horizontalMove = moveSpeed * horizontal;
        var newPos = Vector3.right * horizontalMove;
        var newPosDelta = newPos * Time.deltaTime;
        this.transform.position += newPos * Time.fixedDeltaTime;
        _rigid.velocity = Vector3.SmoothDamp(velocity0, newPos, ref velocity0, smoothing);
    }

    void Flip()
    {
        Vector2 scale = transform.localScale;
        if(_rigid.velocity.x > 0f)
        {
            scale.x = 1f;
        } else if(_rigid.velocity.x < 0f)
        {
            scale.x = -1f;
        }
        transform.localScale = scale;
    }
    void Sprint()
    {
        if(Input.GetButton("Sprint") && !isCrouching)
        {
            isSprinting = true;
            moveSpeed = sprintSpeed;
        } else
        {
            isSprinting = false;
            moveSpeed = walkSpeed;
        }
    }
    void Jump()
    {
        var jump = Input.GetButtonDown("Jump");
        if (jump && isGrounded)
        {
            var force = new Vector2(0f, jumpHeight);
            _rigid.AddForce(force); 
            isJumping = true;
            isGrounded = false;
        }
    }

    void Crouch()
    {

        if(Input.GetButton("Crouch"))
        {
            isCrouching = true;
            jumpHeight = crouchJump;
            c_Collider.size = new Vector2 (originalWidth, crouchHeight);

        } else
        {
            if(crouchHeight != originalHeight)
            {
                isCrouching = true;
            }
            isCrouching = false;
            jumpHeight = originalJump;
            c_Collider.size = new Vector2 (originalWidth, originalHeight);

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            isJumping = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
            isJumping = true;
        }
    }
}
