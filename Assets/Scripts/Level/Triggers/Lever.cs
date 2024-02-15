using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : BaseTrigger
{
    public override void Trigger(PlayerController player)
    {
        base.Trigger(player);
    }

    private bool canTrigger = false;
}
