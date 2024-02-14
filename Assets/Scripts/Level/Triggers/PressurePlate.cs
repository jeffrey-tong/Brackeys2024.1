using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : BaseTrigger
{
    public override void Trigger()
    {
        base.Trigger();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Trigger();
        }
    }
}
