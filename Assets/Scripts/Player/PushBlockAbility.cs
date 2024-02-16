using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlockAbility : PlayerAbility
{
    private PlayerLocomotion m_Locomotion;

    [SerializeField] private float pushDistance = 0.5f;
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private LayerMask moveableLayer;

    private MoveableObject lastHitPushableObject;

    private void Awake()
    {
        m_Locomotion = GetComponent<PlayerLocomotion>();
    }

    public override bool CanPerformAbility()
    {
        return m_Locomotion.IsPlayerGrounded();
    }

    public override void PerformAbility()
    {
        
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * m_Locomotion.GetXInput(), pushDistance, moveableLayer);
        if (hit.collider != null)
        {
            MoveableObject pushObject = hit.collider.GetComponent<MoveableObject>();
            if (pushObject != null)
            {
                pushObject.SetPushable(true, transform.right * m_Locomotion.GetXInput() * pushForce);
                lastHitPushableObject = pushObject;
            }
        }
        if (hit.collider == null || (lastHitPushableObject != null && hit.collider != lastHitPushableObject.GetComponent<Collider2D>()))
        {
            if (lastHitPushableObject != null)
            {
                lastHitPushableObject.SetPushable(false, Vector2.zero);
                lastHitPushableObject = null;
            }
        }

        Debug.DrawRay(transform.position, transform.right * m_Locomotion.GetXInput() * pushDistance, Color.red, 2.0f);
    }
}
