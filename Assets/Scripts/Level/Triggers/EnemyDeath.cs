using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : BaseTrigger
{
    [SerializeField] private AudioClip destructionSound;
    public override void Trigger(PlayerController player = null)
    {
        base.Trigger(player);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.CompareTag("Projectile"))
        {
            OnDestroyed();
        }
    }

    private void OnDestroyed()
    {
        AudioManager.Instance.PlayAudioSFX(destructionSound);
        Trigger();
        Destroy(gameObject);
    }
}
