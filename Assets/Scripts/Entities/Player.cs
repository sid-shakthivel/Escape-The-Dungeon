using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using EntityNamespace;

public class Player : Entity
{
    public int PlayerCoinCount = 0;
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

            if (Input.touchCount == 1)
            {
                Touch tTouch = Input.GetTouch(0);
                if (tTouch.phase == TouchPhase.Began)
                {
                    if (!CheckForChest(tTouch.position) && !EventSystem.current.IsPointerOverGameObject(tTouch.fingerId))
                        FireProjectile();
                }
            }

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

    private bool CheckForChest(Vector3 Position)
    {
        RaycastHit2D Hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Position));

        if (Hit == true && Hit.collider.CompareTag("Crate"))
        {
            Crate CrateScript = Hit.collider.GetComponent<Crate>();
            StartCoroutine(CrateScript.LootCrate());
            return true;
        }
        return false;
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
                SceneManager.LoadScene("Win");
                break;
        }

        if (EntityHeartCount <= 0)
        {
            EntityAnimator.SetTrigger("Death");
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene("Restart");
        }
    }
}
