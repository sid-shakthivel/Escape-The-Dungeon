using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool IsUsed;
    private Player PlayerScript;

    private void Awake()
    {
        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = PlayerObject.GetComponent<Player>();
    }

    private void Start()
    {
        InvokeRepeating("ReplenishSupplies", 0, 30);
    }

    public void LootChest()
    {
        if (!IsUsed)
        {
            int Option = Random.Range(0, 3);
            if (Option == 0)
            {
                // Increase Health
                PlayerScript.SetHearts(PlayerScript.GetHearts() + 10);
            }
            else if (Option == 1)
            {
                // Increase Bullet Count
                PlayerScript.SetBulletCount(PlayerScript.GetBulletCount() + 10);
            }
            else
            {
                // Increase Bullet Power
                PlayerScript.SetBulletPower(PlayerScript.GetBulletPower() + 10);
            }
            IsUsed = true;
        }
        else
        {
            // If During That Time Flash Message
        }
    }

    private void ReplenishSupplies()
    {
        IsUsed = false;
    }
}