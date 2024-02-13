using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerLocomotion : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D m_RigidBody;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;

    [Header("Movement")]
    [SerializeField] private float acceleration = 150f;
    [SerializeField] private float deceleration = 300f;
    [SerializeField] private float maxSpeed = 15.0f;

    [Header("Air Mobility")]
    [SerializeField] private float airDeceleration = 50f;

    [Header("Jump Strength")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float lowJumpMultiplier = 2.5f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float coyoteTime = 0.2f;

    [Header("Ground Check")]
    [SerializeField] private float groundCheckDist = 0.5f;
    [SerializeField] private LayerMask groundMask;

    private float currentSpeed = 0f;
    private float xInput = 0f;

    private float timeSinceLastGrounded = 0f;
    private bool isGrounded = false;

    private float timeSinceJumpPressed = 0f;
    private bool jumpRequest = false;
    private bool isJumping = false;

    private Vector2 desiredVelocity;


    // Anim hashes
    private int animIsMovingHash;

    private void Awake()
    {
        if (m_RigidBody == null) m_RigidBody = GetComponent<Rigidbody2D>();
        if (m_Animator == null) m_Animator = GetComponentInChildren<Animator>();

        animIsMovingHash = Animator.StringToHash("IsMoving");
    }

    private void Update()
    {
        UpdateSpeed();
        CheckGrounded();
        UpdateAnimation();

    }

    private void FixedUpdate()
    {

        HandleAirVelocity();

        float translation = currentSpeed * xInput;
        m_RigidBody.velocity = new Vector2(translation, m_RigidBody.velocity.y);
    }

    private void UpdateSpeed()
    {
        bool isMoving = Mathf.Abs(xInput) > Mathf.Epsilon;
        float targetSpeed = isMoving ? maxSpeed : 0f;
        float targetAccel = isMoving ? acceleration : isGrounded ? deceleration : airDeceleration;

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, targetAccel * Time.deltaTime);
    }

    private void CheckGrounded()
    {
        Vector2 start = m_RigidBody.position;
        Vector2 end = start + (Vector2.down * groundCheckDist);

        RaycastHit2D hit = Physics2D.Linecast(start, end, groundMask);
        bool wasGrounded = hit;

        if (isGrounded && wasGrounded == false)
        {
            timeSinceLastGrounded = Time.time;
            isGrounded = false;
        }

        if (isGrounded == false && wasGrounded)
        {
            isGrounded = true;
            isJumping = false;
        }
    }

    private void UpdateAnimation()
    {
        if (xInput > 0 && m_SpriteRenderer.flipX)
            m_SpriteRenderer.flipX = false;

        if (xInput < 0 && m_SpriteRenderer.flipX == false)
            m_SpriteRenderer.flipX = true;

        if (m_Animator != null)
        {
            bool isMoving = Mathf.Abs(currentSpeed) > Mathf.Epsilon;
            m_Animator.SetBool(animIsMovingHash, isMoving);
        }
    }

    private void HandleAirVelocity()
    {
        if (isGrounded == true) return;

        if (m_RigidBody.velocity.y > 0 && !jumpRequest)
        {
            m_RigidBody.AddForce(Physics2D.gravity * lowJumpMultiplier);
        }

        if (m_RigidBody.velocity.y < 0)
        {
            m_RigidBody.AddForce(Physics2D.gravity * fallMultiplier);
        }
    }

    public void SetXInput(float newInput)
    {
        this.xInput = newInput;
    }

    public void StartJump()
    {
        jumpRequest = true;
        bool checkCoyote = Time.time <= timeSinceLastGrounded + coyoteTime && !isJumping;

        if (isGrounded || checkCoyote)
        {           
            m_RigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = true;
        }

        else
        {
            timeSinceJumpPressed = Time.time;
        }
    }

    public void StopJump()
    {
        jumpRequest = false;
    }
}
