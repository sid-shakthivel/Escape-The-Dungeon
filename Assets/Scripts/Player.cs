using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D Bullet;
    public float Speed;
    public float BulletSpeed;

    private Rigidbody2D PlayerRigidbody;
    private Animator PlayerAnimator;
    private float BulletCount = 10;
    private float Hearts = 10;
    private float BulletPower = 10;

    private void Awake()
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Vector3 MovementInput = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        PlayerAnimator.SetFloat("HorizontalSpeed", Input.GetAxis("Horizontal") * Speed);
        PlayerAnimator.SetFloat("VerticalSpeed", Input.GetAxis("Vertical") * Speed);

        PlayerRigidbody.MovePosition(transform.position + MovementInput * Time.deltaTime * Speed);

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D Hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

            if (Hit == false)
            {
                Transform Bow = gameObject.transform.GetChild(0);
                Transform Pivot = gameObject.transform.GetChild(1);

                Vector3 ShootDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Pivot.transform.position;
                ShootDirection.Normalize();
                float Angle = Mathf.Atan2(ShootDirection.y, ShootDirection.x) * Mathf.Rad2Deg;
                Bow.eulerAngles = new Vector3(0, 0, Angle);
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
