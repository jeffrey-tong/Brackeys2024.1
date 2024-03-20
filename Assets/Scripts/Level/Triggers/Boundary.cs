using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boundary : BaseTrigger
{
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

    }
    public override void Trigger(PlayerController player)
    {
        base.Trigger(player);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Triggerable") || collision.CompareTag("Player"))
        {
            Trigger(null);
            sr.sprite = sprite1;
        }  
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Triggerable") || collision.CompareTag("Player"))
        {
            Trigger(null);
            sr.sprite = sprite2;
        }
    }
}
