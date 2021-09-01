using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody2D ArrowRigidBody;

    private void Awake()
    {
        ArrowRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float Angle = Mathf.Atan2(ArrowRigidBody.velocity.x, ArrowRigidBody.velocity.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(Angle * -1, Vector3.forward);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
