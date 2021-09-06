using System.Collections;
using UnityEngine;
using EnemyNamespace;

public class Mole : Enemy
{
    protected override void Start()
    {
        base.Start();
        EntityHeartCount = 10;
        EntitySpeed = 1;
        InflictedDamage = 2;
        StartCoroutine(GetPath());
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
