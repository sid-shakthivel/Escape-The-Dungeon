using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed;

    private GameObject Player;
    private Rigidbody2D EnemyRigidbody;
    private bool IsPlayerNear;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        EnemyRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        StartCoroutine(MoveTowardsPlayer());
        StartCoroutine(CheckIsPlayerNear());
    }

    private IEnumerator MoveTowardsPlayer()
    {
        if (IsPlayerNear)
        {
            for (float i = 0; i < 1; i += Time.deltaTime / 10)
            {
                EnemyRigidbody.MovePosition(Vector2.MoveTowards(transform.position, Player.transform.position, Speed * Time.deltaTime));
                yield return new WaitForSeconds(30f);
            }
        }
    }

    private IEnumerator CheckIsPlayerNear()
    {
        IsPlayerNear = IsPlayerPositionNear();
        yield return new WaitForSeconds(1f);
    }

    private bool IsPlayerPositionNear()
    {
        if (Vector2.Distance(transform.position, Player.transform.position) <= 5)
            return true;
        return false;
    }
}
