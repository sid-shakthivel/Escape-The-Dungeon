using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D Bullet;
    public float Speed;
    public float BulletSpeed;

    private Rigidbody2D PlayerRigidbody;
    private float BulletCount = 10;
    private float Hearts = 10;
    private float BulletPower = 10;

    private void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector3 MovementInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        PlayerRigidbody.MovePosition(transform.position + MovementInput * Time.deltaTime * Speed);

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D Hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (Hit == false)
            {
                Transform Gun = this.gameObject.transform.GetChild(0);
                Vector3 ShootDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                Rigidbody2D BulletInstance = Instantiate(Bullet, Gun.position, Quaternion.identity);
                BulletInstance.velocity = ShootDirection * BulletSpeed;
            }
            else if (Hit.collider.CompareTag("Chest"))
            {
                Chest ChestScript = Helper.FindClosestGameObject("Chest", transform.position).GetComponent<Chest>();
                ChestScript.LootChest();
            }
        }
    }

    public float GetBulletCount()
    {
        return BulletCount;
    }

    public float GetHearts()
    {
        return Hearts;
    }

    public float GetBulletPower()
    {
        return BulletPower;
    }

    public void SetBulletCount(float NewBulletCount)
    {
        BulletCount = NewBulletCount;
    }

    public void SetHearts(float NewHealth)
    {
        Hearts = NewHealth;
    }

    public void SetBulletPower(float NewBulletPower)
    {
        BulletPower = NewBulletPower;
    }
}
