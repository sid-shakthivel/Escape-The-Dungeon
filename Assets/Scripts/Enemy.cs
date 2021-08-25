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
        Vector2 DirectionToPlayer = (Player.transform.position - transform.position).normalized;
        EnemyAnimator.SetFloat("HorizontalSpeed", Mathf.Round(DirectionToPlayer.x));
        EnemyAnimator.SetFloat("VerticalSpeed", Mathf.Round(DirectionToPlayer.y));
        Debug.Log(DirectionToPlayer);
        EnemyRigidbody.velocity = new Vector2(DirectionToPlayer.x, DirectionToPlayer.y) * Speed;
    }
}
