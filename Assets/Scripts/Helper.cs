using UnityEngine;

public static class Helper
{
    public static GameObject FindClosestGameObject(string TargetObject, Vector2 Position)
    {
        GameObject ClosestObject = null;
        float LowestDistance = 100000;
        foreach (GameObject Object in GameObject.FindGameObjectsWithTag(TargetObject))
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

    public static GameObject FindClosestGameObject(GameObject TargetObject, Vector3 Position)
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
