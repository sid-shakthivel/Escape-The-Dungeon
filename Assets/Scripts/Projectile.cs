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

    private void Awake()
    {
        ProjectileRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float Angle = Mathf.Atan2(ProjectileRigidBody.velocity.x, ProjectileRigidBody.velocity.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(Angle * -1, Vector3.forward);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
