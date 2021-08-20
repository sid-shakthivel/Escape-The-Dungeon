using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.gameObject.CompareTag("Dungeon"))
        {
            Destroy(Collision.collider);
            Destroy(Collision.otherCollider);
        }
    }
}
