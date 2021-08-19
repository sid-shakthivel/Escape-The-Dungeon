using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D Bullet;
    public float Speed;
    public float BulletSpeed;

    private float BulletCount;
    private float Health = 100;
    private float BulletPower = 10;

    private void FixedUpdate()
    {
        transform.Translate(Input.GetAxis("Horizontal") * Speed * Time.deltaTime, 0, 0);
        transform.Translate(0, Input.GetAxis("Vertical") * Speed * Time.deltaTime, 0);

        if (Input.GetMouseButtonUp(0))
        {
            Transform Gun = this.gameObject.transform.GetChild(0);

            Vector3 ShootDirection = Input.mousePosition;
            ShootDirection = Camera.main.ScreenToWorldPoint(ShootDirection);
            ShootDirection = ShootDirection - transform.position;

            Rigidbody2D BulletInstance = Instantiate(Bullet, Gun.position, Quaternion.identity);
            BulletInstance.velocity = ShootDirection * BulletSpeed;
        }
    }
}
