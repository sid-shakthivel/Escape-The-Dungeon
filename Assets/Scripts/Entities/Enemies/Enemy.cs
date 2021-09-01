using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    protected GameObject PlayerGameObject;
    protected float InflictedDamage;

    protected override void Move()
    {
        MovementVector = (PlayerGameObject.transform.position - transform.position).normalized;
        float DistanceToPlayer = Vector2.Distance(PlayerGameObject.transform.position, transform.position);

        if (DistanceToPlayer < 10 && DistanceToPlayer > 3)
            FireProjectile();
        else if (DistanceToPlayer < 10)
            EntityRigidbody.velocity = new Vector2(MovementVector.x, MovementVector.y) * EntitySpeed;
    }

    protected override IEnumerator OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.gameObject.CompareTag("Arrow"))
        {
            EntityHeartCount -= InflictedDamage;
            if (EntityHeartCount <= 0)
            {
                EntityAnimator.SetBool("IsDead", true);
                yield return new WaitForSeconds(10);
                Destroy(gameObject);
            }
        }
    }
}
