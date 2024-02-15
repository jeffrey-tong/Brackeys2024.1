using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : BaseTrigger
{
    public override void Trigger(PlayerController player = null)
    {
        base.Trigger(player);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            OnDestroyed();
        }
    }

    private void OnDestroyed()
    {
        Trigger();
        Destroy(gameObject);
    }
}
