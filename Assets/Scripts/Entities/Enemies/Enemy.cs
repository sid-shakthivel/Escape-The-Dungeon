using System.Collections;
using UnityEngine;
using EntityNamespace;

namespace EnemyNamespace
{
    public class Enemy : Entity
    {
        protected GameObject PlayerGameObject;
        protected float InflictedDamage;
        protected float DistanceToPlayer;

        protected override void Move()
        {
            PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
            MovementVector = (PlayerGameObject.transform.position - transform.position).normalized;
            DistanceToPlayer = Vector2.Distance(PlayerGameObject.transform.position, transform.position);

            if (DistanceToPlayer <= 5)
            {
                MovementVector = Vector2.zero;
                EntityRigidbody.velocity = MovementVector;
            }
            else if (DistanceToPlayer <= 15)
                EntityRigidbody.velocity = MovementVector * EntitySpeed;
        }

        protected override IEnumerator OnCollisionEnter2D(Collision2D Collision)
        {
            if (Collision.gameObject.CompareTag("Arrow"))
            {
                EntityHeartCount -= InflictedDamage;
                Destroy(Collision.gameObject);
                if (EntityHeartCount <= 0)
                {
                    EntityAnimator.SetBool("IsDead", true);
                    yield return new WaitForSeconds(2);
                    Destroy(gameObject);
                }
            }
        }
    }
}
