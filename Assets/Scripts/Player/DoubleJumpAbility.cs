﻿using System;
using UnityEngine;

public class DoubleJumpAbility : PlayerAbility
{
    private PlayerLocomotion m_Locomotion;
    private bool canDoubleJump = true;

    [SerializeField] private float jumpVelocity = 5f;

    [SerializeField] private AudioClip jumpSound;

    private void Awake()
    {
        m_Locomotion = GetComponent<PlayerLocomotion>();
    }

    private void Start()
    {
        m_Locomotion.OnPlayerLanded += Locomotion_OnLandedCallback;
    }

    private void Locomotion_OnLandedCallback()
    {
        canDoubleJump = true;
    }

    public override bool CanPerformAbility()
    {
        return (m_Locomotion.GetVelocity().y < 0
            && !m_Locomotion.IsPlayerGrounded()
            && canDoubleJump == true
            );
    }

    public override void PerformAbility()
    {
        m_Locomotion.SetVelocityY(jumpVelocity);
        canDoubleJump = false;
        AudioManager.Instance.PlayAudioSFX(jumpSound);
    }
}