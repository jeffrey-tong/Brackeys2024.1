using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float projectileDuration = 1f;
    private Vector2 projectileDirection = Vector2.zero;

    public void Shoot(Vector2 direction)
    {
        projectileDirection = direction;

        Invoke("DestroyProjectile", projectileDuration);
    }

    private void Update()
    {
        transform.Translate(projectileDirection * projectileSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyProjectile();
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
