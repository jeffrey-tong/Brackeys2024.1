using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isPushable = false;
    private Vector2 pushDirection = Vector2.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetPushable(false, pushDirection);
    }

    public void SetPushable(bool pushable, Vector2 direction)
    {
        isPushable = pushable;
        if (isPushable)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        pushDirection = direction;
    }
}
