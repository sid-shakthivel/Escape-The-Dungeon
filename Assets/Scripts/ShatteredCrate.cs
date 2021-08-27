using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatteredCrate : MonoBehaviour
{
    private Rigidbody2D[] PlankRigidbodies;

    private void Start()
    {
        PlankRigidbodies = GetComponentsInChildren<Rigidbody2D>();

        foreach (Rigidbody2D PlankRigidbody in PlankRigidbodies)
        {
            float RandomTorque = Random.Range(-100f, 100f);
            float RandomX = Random.Range(-100f, 100f);
            float RandomY = Random.Range(-100f, 100f); ;
            PlankRigidbody.AddTorque(RandomTorque);
            PlankRigidbody.AddForce(new Vector2(RandomX, RandomY));
        }
    }
}
