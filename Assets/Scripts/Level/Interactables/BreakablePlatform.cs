using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BreakablePlatform : BaseInteractable
{
    private SpriteRenderer sr;
    private BoxCollider2D boxCollider;
    [SerializeField] private float platformLength;
    [SerializeField] private float platformWidth;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        sr.size = new Vector2(platformLength, platformWidth);
        boxCollider.size = new Vector2(platformLength, platformWidth);
    }
    public override void Activate()
    {
        
    }

    private void OnValidate()
    {
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        sr.size = new Vector2(platformLength, platformWidth);
        boxCollider.size = new Vector2(platformLength, platformWidth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }
}
