using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public GameObject ShatteredCrate;
    public GameObject Heart;
    public GameObject Coin;
    public GameObject ArrowPickup;

    private GameObject ClosestShatteredCrate;

    public IEnumerator LootCrate()
    {
        Vector3 ChestPosition = transform.position;
        Destroy(gameObject);
        ClosestShatteredCrate = Instantiate(ShatteredCrate, ChestPosition, Quaternion.identity);

        yield return new WaitForSeconds(1);
        Destroy(ClosestShatteredCrate);

        int Option = Random.Range(0, 3);
        switch (Option)
        {
            case 0:
                Instantiate(Coin, ChestPosition, Quaternion.identity);
                break;
            case 1:
                Instantiate(Heart, ChestPosition, Quaternion.identity);
                break;
            case 2:
                Instantiate(ArrowPickup, ChestPosition, Quaternion.identity);
                break;
        }
    }
}
