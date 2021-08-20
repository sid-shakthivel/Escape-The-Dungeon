using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool IsUsed;
    private Player PlayerScript;
    
    private void Awake()
    {
        //GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        //Player PlayerScript = PlayerObject.GetComponent<Player>();
    }

    private void Update()
    {
        if (!IsUsed)
        {
            IsUsed = true;
        }
    }
}
