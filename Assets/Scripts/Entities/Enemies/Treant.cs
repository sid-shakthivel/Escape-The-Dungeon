using System.Collections;
using UnityEngine;

public class Treant : Enemy
{
    protected override void Start()
    {
        EntityProjectileCount = Mathf.Infinity;
        EntitySpeed = 2.5f;
        InflictedDamage = ProjectileRigidbody.GetComponent<Projectile>().ProjectileDamage;
        StartCoroutine("FireProjectileEveryInterval", 5);
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
