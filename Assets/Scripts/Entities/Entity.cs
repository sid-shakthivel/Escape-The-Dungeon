using System.Collections;
using UnityEngine;

namespace EntityNamespace
{
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

        public Rigidbody2D ProjectileRigidbody;
        public float ProjectileSpeed;
        public virtual float EntityHeartCount { get; set; }
        public virtual float EntityProjectileCount { get; set; }

        protected Rigidbody2D EntityRigidbody;
        protected Animator EntityAnimator;
        protected virtual float EntitySpeed { get; set; }
        protected EntityState EntityCurrentState = EntityState.Idle;
        protected Vector3 MovementVector;
        protected Rigidbody2D InstanitatedProjectile;

        protected void Awake()
        {
            EntityAnimator = GetComponent<Animator>();
            EntityRigidbody = GetComponent<Rigidbody2D>();
            StartCoroutine(EntityLoop());
        }

        protected virtual void Start() { }

        protected virtual IEnumerator EntityLoop()
        {
            for (; ; )
            {
                Move();
                SetAnimation();
                SetState();
                yield return new WaitForSeconds(0.000001f);
            }
        }

        protected void SetAnimation()
        {
            EntityAnimator.SetFloat("HorizontalSpeed", Mathf.Round(MovementVector.x));
            EntityAnimator.SetFloat("VerticalSpeed", Mathf.Round(MovementVector.y));
            EntityAnimator.SetBool("IsAttack", false);
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

        protected virtual void FireProjectile()
        {
            if (EntityProjectileCount > 0)
            {
                EntityAnimator.SetBool("IsAttack", true);

                switch (EntityCurrentState)
                {
                    case EntityState.Up:
                        InstanitatedProjectile = Instantiate(ProjectileRigidbody, transform.position + new Vector3(0, 0.75f, 0), Quaternion.Euler(0, 0, 0));
                        InstanitatedProjectile.velocity = ProjectileSpeed * Vector2.up;
                        break;
                    case EntityState.Right:
                        InstanitatedProjectile = Instantiate(ProjectileRigidbody, transform.position + new Vector3(0.75f, 0, 0), Quaternion.Euler(0, 0, -90));
                        InstanitatedProjectile.velocity = ProjectileSpeed * Vector2.right;
                        break;
                    case EntityState.Left:
                        InstanitatedProjectile = Instantiate(ProjectileRigidbody, transform.position - new Vector3(0.75f, 0, 0), Quaternion.Euler(0, 0, 90));
                        InstanitatedProjectile.velocity = ProjectileSpeed * Vector2.left;
                        break;
                    default:
                        InstanitatedProjectile = Instantiate(ProjectileRigidbody, transform.position - new Vector3(0, 0.75f, 0), Quaternion.Euler(0, 0, 180));
                        InstanitatedProjectile.velocity = ProjectileSpeed * Vector2.down;
                        break;
                }

                --EntityProjectileCount;
            }
        }

        protected virtual IEnumerator OnCollisionEnter2D(Collision2D Collision)
        {
            Debug.Log("Collision!");
            EntityHeartCount -= 1;
            if (EntityHeartCount <= 0)
            {
                EntityAnimator.SetTrigger("Death");
                yield return new WaitForSeconds(2);
                Destroy(gameObject);
            }
        }
    }

}

