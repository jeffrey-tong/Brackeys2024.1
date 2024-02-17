using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingAbility : PlayerAbility
{
    private PlayerLocomotion m_Locomotion;
    private PlayerController controller;
    private Vector3 defaultScale = Vector3.one;
    [SerializeField] private float shrinkFactor = 0.5f;

    private void Start()
    {
        m_Locomotion = GetComponent<PlayerLocomotion>();
        controller = GetComponent<PlayerController>();
    }

    public override bool CanPerformAbility()
    {
        return m_Locomotion.IsPlayerGrounded();
    }

    public override void PerformAbility()
    {
        if(controller.transform.localScale.magnitude >= defaultScale.magnitude)
        {
            ShrinkCharacter();
        }
        else
        {
            ExpandCharacter();
        }
    }

    private void ShrinkCharacter()
    {
        Vector3 newScale = new Vector3(defaultScale.x * shrinkFactor, defaultScale.y * shrinkFactor, defaultScale.z * shrinkFactor);
        controller.transform.localScale = newScale;
    }

    private void ExpandCharacter()
    {
        controller.transform.localScale = defaultScale;
    }
}
