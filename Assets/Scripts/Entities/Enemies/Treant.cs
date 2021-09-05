using System.Collections;
using UnityEngine;
using EnemyNamespace;

public class Treant : Enemy
{
    protected override void Start()
    {
        base.Start();
        EntityHeartCount = 5;
        EntityProjectileCount = Mathf.Infinity;
        InflictedDamage = ProjectileRigidbody.GetComponent<Projectile>().ProjectileDamage;
        StartCoroutine("FireProjectileEveryInterval", 1);
    }

    protected override void Move()
    {
        base.Move();
        if (DistanceToPlayer <= 5)
        {
            MovementVector = Vector2.zero;
            EntityRigidbody.velocity = MovementVector;
        }
        else if (DistanceToPlayer <= 15)
            EntityRigidbody.velocity = MovementVector * EntitySpeed;
    }

    private IEnumerator FireProjectileEveryInterval(float Interval)
    {
        for (;;)
        {
            if (DistanceToPlayer <= 5)
            {
                FireProjectile();
                yield return new WaitForSeconds(5);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
