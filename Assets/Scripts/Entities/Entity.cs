using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected enum EntityState
    {
        Idle,
        Up,
        Down,
        Right,
        Left
    }

    public Rigidbody2D Projectile;
    public float ProjectileSpeed;

    protected Rigidbody2D EntityRigidbody;
    protected Animator EntityAnimator;
    protected virtual float EntitySpeed { get; set; }
    protected virtual float EntityHeartCount { get; set; }
    protected EntityState EntityCurrentState = EntityState.Idle;
    protected Vector3 MovementVector;
    
    protected void Awake()
    {
        EntityAnimator = GetComponent<Animator>();
        EntityRigidbody = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        StartCoroutine(EntityLoop());
    }

    protected virtual IEnumerator EntityLoop()
    {
        for (;;)
        {
            Move();
            SetAnimation();
            SetState();
            yield return new WaitForSeconds(0.1f);
        }
    }

    protected void SetAnimation()
    {
        EntityAnimator.SetFloat("HorizontalSpeed", Mathf.Round(MovementVector.x));
        EntityAnimator.SetFloat("VerticalSpeed", Mathf.Round(MovementVector.y));
        EntityAnimator.SetBool("IsAttack", false);
        EntityAnimator.SetBool("IsDead", false);
    }

    protected virtual void Move()
    {
        MovementVector = new Vector3(1, 0, 0);
        EntityRigidbody.MovePosition(transform.position + MovementVector * Time.deltaTime * EntitySpeed);
    }

    protected void SetState()
    {
        if (EntityAnimator.GetFloat("HorizontalSpeed") < -0.01) EntityCurrentState = EntityState.Left;
        if (EntityAnimator.GetFloat("HorizontalSpeed") > 0.01) EntityCurrentState = EntityState.Right;
        if (EntityAnimator.GetFloat("VerticalSpeed") < -0.01) EntityCurrentState = EntityState.Down;
        if (EntityAnimator.GetFloat("VerticalSpeed") > 0.01) EntityCurrentState = EntityState.Up;
    }

    protected void FireProjectile()
    {
        EntityAnimator.SetBool("IsAttack", true);
        Rigidbody2D InstanitatedProjectile = Instantiate(Projectile, transform.position, Quaternion.identity);

        switch (EntityCurrentState)
        {
            case EntityState.Up:
                InstanitatedProjectile.velocity = ProjectileSpeed * Vector2.up;
                break;
            case EntityState.Right:
                InstanitatedProjectile.velocity = ProjectileSpeed * Vector2.right;
                break;
            case EntityState.Left:
                InstanitatedProjectile.velocity = ProjectileSpeed * Vector2.left;
                break;
            default:
                InstanitatedProjectile.velocity = ProjectileSpeed * Vector2.down;
                break;
        }
    }

    protected virtual IEnumerator OnCollisionEnter2D(Collision2D Collision)
    {
        Debug.Log("Collision!");
        EntityHeartCount -= 1;
        if (EntityHeartCount <= 0)
        {
            EntityAnimator.SetBool("IsDead", true);
            yield return new WaitForSeconds(10);
            Destroy(gameObject);
        }
    }
}
