using System.Collections;
using UnityEngine;
using EntityNamespace;

public class Player : Entity
{
    public int PlayerCoinCount = 0;

    private Touch touch;
    private float TouchDuration;
    private UIController Canvas;

    protected override void Start()
    {
        EntitySpeed = 5f;
        EntityHeartCount = 10;
        EntityProjectileCount = 20;
        Canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();
    }

    protected override IEnumerator EntityLoop()
    {
        for (;;)
        {
            Move();
            SetAnimation();
            SetState();

            if (Input.touchCount > 0)
            {
                TouchDuration += Time.deltaTime;
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Ended && TouchDuration < 0.2f)
                    StartCoroutine("SingleOrDoubleTap");
            }
            else
                TouchDuration = 0f;

            if (Input.GetMouseButtonUp(0))
                CheckForChest(Input.mousePosition);

            if (Input.GetButtonDown("Fire"))
                FireProjectile();

            yield return new WaitForSeconds(0.000001f);
        }
    }

    protected override void Move()
    {
        MovementVector = new Vector3(SimpleInput.GetAxis("Horizontal"), SimpleInput.GetAxis("Vertical"), 0);
        EntityRigidbody.MovePosition(transform.position + MovementVector * Time.deltaTime * EntitySpeed);
    }

    IEnumerator SingleOrDoubleTap()
    {
        yield return new WaitForSeconds(0.3f);
        if (touch.tapCount == 1)
            FireProjectile();
        else
        {
            StopCoroutine("SingleOrDoubleTap");
            CheckForChest(touch.position);
        }
    }

    private void CheckForChest(Vector3 Position)
    {
        RaycastHit2D Hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Position));

        if (Hit == true && Hit.collider.CompareTag("Crate"))
        {
            Crate CrateScript = Hit.collider.GetComponent<Crate>();
            StartCoroutine(CrateScript.LootCrate());
        }
    }

    protected override IEnumerator OnCollisionEnter2D(Collision2D Collision)
    {
        switch (Collision.gameObject.tag)
        {
            case "Mole":
                EntityHeartCount -= 1;
                Canvas.RemoveHeart();
                break;
            case "EnemyAmmo":
                Destroy(Collision.gameObject);
                EntityHeartCount -= Collision.gameObject.GetComponent<Projectile>().ProjectileDamage;
                Canvas.RemoveHeart();
                break;
            case "Coin":
                Destroy(Collision.gameObject);
                PlayerCoinCount += 1;
                break;
            case "Heart":
                Destroy(Collision.gameObject);
                if (EntityHeartCount < 10)
                    EntityHeartCount += 1;
                Canvas.AddHeart();
                break;
            case "ArrowPickup":
                Destroy(Collision.gameObject);
                EntityProjectileCount += 10;
                break;
            case "Key":
                Destroy(Collision.gameObject);
                Debug.Log("You've Won!");
                break;
        }

        if (EntityHeartCount <= 0)
        {
            EntityAnimator.SetBool("IsDead", true);
            yield return new WaitForSeconds(10);
            if (gameObject.CompareTag("Player") == false)
                Destroy(gameObject);
        }
    }
}
