using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : BaseTrigger
{
    public override void Trigger(PlayerController player = null)
    {
        base.Trigger(player);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out PlayerController player))
                Trigger(player);
        }

        if (collision.CompareTag("Triggerable"))
        {
            if (!collision.isTrigger)
            {
                Trigger(null);
            }   
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        // Need to not invoke delegates
    }
}
