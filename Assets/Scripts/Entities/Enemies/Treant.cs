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

        StartCoroutine("FireProjectileEveryInterval", 2.5);
        StartCoroutine(GetPath());
    }

    protected override void Move()
    {
        PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
        DistanceToPlayer = Vector2.Distance(PlayerGameObject.transform.position, transform.position);
        
        if (DistanceToPlayer > 15 || DistanceToPlayer <= 3)
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
                yield return new WaitForSeconds(Interval);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    protected override IEnumerator GetPath()
    {
        for (; ; )
        {
            if (DistanceToPlayer <= 15 && DistanceToPlayer > 3)
            {
                Tile CurrentTile = GameGenerator.Graph.First(Node => Node.Position == FloorTilemap.WorldToCell(transform.position));
                Tile PlayerTile = GameGenerator.Graph.First(Node => Node.Position == FloorTilemap.WorldToCell(PlayerGameObject.transform.position));

                Djkstra djkstra = new Djkstra(GameGenerator.Graph, CurrentTile);

                Path = djkstra.GetShortestPath(PlayerTile, CurrentTile);
                PathIndex = 0;
            }

            yield return new WaitForSeconds(5);
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

    protected override IEnumerator OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.gameObject.CompareTag("Arrow"))
        {
            EntityHeartCount -= InflictedDamage;
            if (EntityHeartCount <= 0)
            {
                EntityAnimator.SetTrigger("Death");
                yield return new WaitForSeconds(0.5f);
                Instantiate(Drops[Random.Range(0, Drops.Count)], transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        } else if (Collision.gameObject.CompareTag("Crate"))
        {
            Destroy(Collision.gameObject);
        }
    }
}
