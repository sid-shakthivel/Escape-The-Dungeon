using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D ProjectileRigidBody;
    [SerializeField]
    private float projectileDamage;

    public float ProjectileDamage
    {
        get { return projectileDamage; }
        set { projectileDamage = value; }
    }

    private void Start()
    {
        ProjectileRigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        yield return new WaitForSeconds(0.5f);
        float Angle = Mathf.Atan2(ProjectileRigidBody.velocity.x, ProjectileRigidBody.velocity.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(Angle * -1, Vector3.forward);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        Collision.rigidbody.AddForce((ProjectileRigidBody.velocity / 2));
    }
}