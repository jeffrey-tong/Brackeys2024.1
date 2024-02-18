using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAbility : PlayerAbility
{
    private PlayerLocomotion m_Locomotion;
    [SerializeField] private Projectile fireballPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private AudioClip fireballSound;

    private void Awake()
    {
        m_Locomotion = GetComponent<PlayerLocomotion>();
    }
    public override bool CanPerformAbility()
    {
        return true;
    }

    public override void PerformAbility()
    {
        Projectile fireballProjectile = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        fireballProjectile.Shoot(Vector2.right * m_Locomotion.GetPlayerFacing());
        AudioManager.Instance.PlayAudioSFX(fireballSound);
    }
}
