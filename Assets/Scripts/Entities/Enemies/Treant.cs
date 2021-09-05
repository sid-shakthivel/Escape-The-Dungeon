using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
        DistanceToPlayer = Vector2.Distance(PlayerGameObject.transform.position, transform.position);
        
        if (DistanceToPlayer > 15 || DistanceToPlayer <= 5)
        {
            MovementVector = Vector3.zero;
            TurnTowardPlayer();
        } else
            base.Move();
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

    private void TurnTowardPlayer()
    {
        Vector3 VectorToPlayer = PlayerGameObject.transform.position - transform.position;
        if (Mathf.Abs(VectorToPlayer.x) > Mathf.Abs(VectorToPlayer.y))
        {
            if (VectorToPlayer.x < 0)
            {
                EntityAnimator.SetTrigger("Left");
                EntityCurrentState = EntityState.Left;
            }
            else
            {
                EntityAnimator.SetTrigger("Right");
                EntityCurrentState = EntityState.Right;
            }
        }
        else
        {
            if (VectorToPlayer.y < 0)
            {
                EntityAnimator.SetTrigger("Down");
                EntityCurrentState = EntityState.Down;
            }
            else
            {
                EntityAnimator.SetTrigger("Up");
                EntityCurrentState = EntityState.Up;
            }
        }
    }
}
