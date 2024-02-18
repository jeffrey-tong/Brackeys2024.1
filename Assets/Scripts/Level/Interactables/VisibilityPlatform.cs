using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityPlatform : BaseInteractable
{
    [SerializeField] private bool isVisible = true;

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

        gameObject.SetActive(isVisible);
    }

    public override void Activate()
    {
        base.Activate();
        isVisible = !isVisible;
        gameObject.SetActive(isVisible);
    }

    private void OnValidate()
    {
        sr = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        sr.size = new Vector2(platformLength, platformWidth);
        boxCollider.size = new Vector2(platformLength, platformWidth);
    }
}
