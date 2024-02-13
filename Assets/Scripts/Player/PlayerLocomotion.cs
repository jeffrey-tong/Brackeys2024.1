﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField] private float airAcceleration = 50f;
    [SerializeField] private float airDeceleration = 50f;
    [SerializeField] private float maxFallSpeed = -15f;

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
    private static int animIsMovingHash;
    private static int animGroundHash;
    private static int animYVelHash;
    private static int animJumpHash;

    private void Awake()
    {
        if (m_RigidBody == null) m_RigidBody = GetComponent<Rigidbody2D>();
        if (m_Animator == null) m_Animator = GetComponentInChildren<Animator>();

        animIsMovingHash = Animator.StringToHash("IsMoving");
        animGroundHash = Animator.StringToHash("Ground");
        animYVelHash = Animator.StringToHash("YVel");
        animJumpHash = Animator.StringToHash("Jump");
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

        float translation = currentSpeed;
        desiredVelocity.x = translation;

        m_RigidBody.velocity = desiredVelocity;
    }

    private void UpdateSpeed()
    {
        bool isMoving = Mathf.Abs(xInput) > Mathf.Epsilon;
        float targetSpeed = isMoving ? maxSpeed * xInput: 0f;

        float accel = isGrounded ? acceleration : airAcceleration;
        float decel = isGrounded ? deceleration : airDeceleration;

        float targetAccel = isMoving ? accel : decel;

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

        m_Animator.SetBool(animGroundHash, isGrounded);
        m_Animator.SetFloat(animYVelHash, m_RigidBody.velocity.y);
    }

    private void HandleAirVelocity()
    {
        if (isGrounded == true && isJumping == false)
        {
            desiredVelocity.y = -2;
            return;

        }

        float gravity = -Physics2D.gravity.y;

        if (m_RigidBody.velocity.y > 0 && !jumpRequest)
        {
            gravity *= lowJumpMultiplier;
        }

        if (m_RigidBody.velocity.y < 0)
        {
            gravity *= fallMultiplier;
        }

        desiredVelocity.y = Mathf.MoveTowards(desiredVelocity.y, maxFallSpeed, gravity * Time.fixedDeltaTime);
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
            desiredVelocity.y = jumpForce;
            m_Animator.SetTrigger(animJumpHash);
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

