using System.Collections;
using UnityEngine;

public class Player : Entity
{
    private Touch touch;
    private float TouchDuration;

    protected override void Start()
    {
        EntitySpeed = 5;
        EntityHeartCount = 10;
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

            yield return new WaitForSeconds(0.01f);
        }
    }

    protected override void Move()
    {
        MovementVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
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
                EntityHeartCount -= 2;
                break;
            case "Treant":
                EntityHeartCount -= 1;
                break;
        }

        if (EntityHeartCount <= 0)
        {
            EntityAnimator.SetBool("IsDead", true);
            yield return new WaitForSeconds(10);
            Destroy(gameObject);
        }
    }
}
