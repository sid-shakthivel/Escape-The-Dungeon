using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Rigidbody2D ProjectileRigidBody;
    [SerializeField]
    protected float projectileDamage;

    public float ProjectileDamage
    {
        get { return projectileDamage; }
        set { projectileDamage = value; }
    }

    protected virtual void Start()
    {
        ProjectileRigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {

        if (gameObject.CompareTag("Arrow") == true && !Collision.gameObject.CompareTag("Player") == true)
            Destroy(gameObject);

        if (gameObject.CompareTag("EnemyAmmo") == true && !Collision.gameObject.CompareTag("Treant") == true)
            Destroy(gameObject);
    }
}
