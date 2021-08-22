using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public static GameObject FindClosestGameObject(GameObject TargetObject, Vector2 Position)
    {
        GameObject ClosestObject = null;
        float LowestDistance = 100000;
        foreach (GameObject Object in GameObject.FindGameObjectsWithTag(TargetObject.tag))
        {
            float Distance = Vector2.Distance(Object.transform.position, Position);
            if (Distance < LowestDistance)
            {
                LowestDistance = Distance;
                ClosestObject = Object;
            }
        }
        return ClosestObject;
    }
}
