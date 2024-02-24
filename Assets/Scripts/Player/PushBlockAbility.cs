using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlockAbility : PlayerAbility
{
    private PlayerLocomotion m_Locomotion;

    [SerializeField] private float pushDistance = 0.5f;
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private float yOffset = 0.25f;
    [SerializeField] [Range(0, 1)] private float moveStrength = 0.6f;
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
        Vector3 rayStart = transform.position;
        rayStart.y += yOffset;

        RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.right * m_Locomotion.GetXInput(), pushDistance, moveableLayer);
        if (hit.collider != null)
        {
            MoveableObject pushObject = hit.collider.GetComponent<MoveableObject>();
            if (pushObject != null)
            {
                pushObject.SetPushable(true, transform.right * m_Locomotion.GetXInput() * pushForce);
                lastHitPushableObject = pushObject;

                Vector2 velocity = hit.rigidbody.velocity;
                velocity.x = m_Locomotion.GetVelocity().x * moveStrength;
                hit.rigidbody.velocity = velocity;
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

    private void OnDrawGizmosSelected()
    {
        Vector3 start = transform.position;
        start.y += yOffset;

        Vector3 end = start;
        end.x += pushDistance;

        Gizmos.DrawLine(start, end);
    }
}
