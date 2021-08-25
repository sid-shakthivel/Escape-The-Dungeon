using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed;

    private GameObject Player;
    private Rigidbody2D EnemyRigidbody;
    private Animator EnemyAnimator;
    private bool IsPlayerNear;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        EnemyAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 XDirection = new Vector2((transform.position - Player.transform.position).x, 0);
        Vector2 YDirection = new Vector2(0, (transform.position - Player.transform.position).y);

        if (Mathf.Abs(XDirection.x) < Mathf.Abs(YDirection.y))
        {
            Speed = XDirection.x < 0 ? -Speed : Speed;
            EnemyRigidbody.MovePosition(new Vector2(Speed * Time.deltaTime, 0));
            EnemyAnimator.SetFloat("HorizontalSpeed", Speed);
        } else
        {
            Speed = YDirection.x < 0 ? -Speed : Speed;
            EnemyRigidbody.MovePosition(new Vector2(0, Speed * Time.deltaTime));
            EnemyAnimator.SetFloat("HorizontalSpeed", Speed);
        }
    }
}
