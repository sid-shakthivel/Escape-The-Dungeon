using UnityEngine;

public class Treant : Enemy
{
    protected override void Start()
    {
        PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
        EntitySpeed = 2.5f;
        InflictedDamage = ProjectileRigidbody.GetComponent<Projectile>().ProjectileDamage;
    }

    protected override void FireProjectile()
    {
        Rigidbody2D InstaniatedProjectile = Instantiate(ProjectileRigidbody, transform.position, Quaternion.identity);
        InstaniatedProjectile.velocity = (PlayerGameObject.transform.position - InstaniatedProjectile.transform.position).normalized * EntitySpeed;
    }
}
