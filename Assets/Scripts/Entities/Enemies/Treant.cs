using System.Collections;
using UnityEngine;
using EnemyNamespace;

public class Treant : Enemy
{
    protected override void Start()
    {
        base.Start();
        EntityProjectileCount = Mathf.Infinity;
        EntitySpeed = 2.5f;
        InflictedDamage = ProjectileRigidbody.GetComponent<Projectile>().ProjectileDamage;
        StartCoroutine("FireProjectileEveryInterval", 2.5);
    }

    private IEnumerator FireProjectileEveryInterval(float Interval)
    {
        for (; ;)
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
