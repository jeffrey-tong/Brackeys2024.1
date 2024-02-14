using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : BaseTrigger
{
    public override void Trigger()
    {
        base.Trigger();
    }

    private bool canTrigger = false;
    private void Update()
    {
        if (canTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E)){
                Trigger();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canTrigger = false;
        }
    }
}
