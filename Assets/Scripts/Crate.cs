using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public GameObject ShatteredCrate;
    public GameObject Heart;
    public GameObject Coin;
    public GameObject ArrowPickup;
    

    private bool IsUsed;
    private Player PlayerScript;

    private void Awake()
    {
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = PlayerObject.GetComponent<Player>();
    }

    public void LootCrate()
    {
        GameObject ClosestShatteredCrate = Instantiate(ShatteredCrate, transform.position, Quaternion.identity);
        Destroy(gameObject);
        StartCoroutine(DestroyShatteredChest(ClosestShatteredCrate));

        int Option = Random.Range(0, 3);
        switch (Option)
        {
            case 0:
                Instantiate(Coin, transform.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(Heart, transform.position, Quaternion.identity);
                break;
            case 2:
                Instantiate(ArrowPickup, transform.position, Quaternion.identity);
                break;
        }
    }

    IEnumerator DestroyShatteredChest(GameObject ClosestShatteredChest)
    {
        yield return new WaitForSeconds(1);
        Destroy(ClosestShatteredChest);
    }
}
