using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool IsUsed;


    void Start()
    {
        
    }

    void Update()
    {
        if (!IsUsed)
        {
            IsUsed = true;
        }
    }
}
