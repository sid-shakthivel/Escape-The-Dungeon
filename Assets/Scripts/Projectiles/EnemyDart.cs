using UnityEngine;

public class EnemyDart : Projectile
{
    private GameObject PlayerGameObject;
    [SerializeField]
    private float EnemyDartSpeed;

    protected override void Start()
    {
        base.Start();
        PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void Update()
    {
        ProjectileRigidBody.velocity = (PlayerGameObject.transform.position - transform.position).normalized * EnemyDartSpeed;
    }
}
