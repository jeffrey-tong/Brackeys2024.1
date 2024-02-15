using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boundary : BaseTrigger
{
    public override void Trigger(PlayerController player)
    {
        base.Trigger(player);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Trigger(null);
        }
    }
}
