using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private DungeonGenerator DungeonGeneratorScript;
    private GameObject PlayerObject;

    private Vector2 Target;

    private void Awake()
    {
        GameObject DungeonGeneratorObject = GameObject.FindGameObjectWithTag("DungeonGenerator");
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
        DungeonGeneratorScript = DungeonGeneratorObject.GetComponent<DungeonGenerator>();

        
    }

    private void DoThing()
    {
        foreach (var Item in DungeonGeneratorScript.DungeonHashMap)
            Target = Item.Key;
    }

    private float GetShortestPath(List<DungeonNode> Dungeons)
    {
        foreach (var Item in Dungeons)
        {
            if (Item.Position == Target)
                return Item.Distance;
            else
                return Item.Distance + GetShortestPath(DungeonGeneratorScript.DungeonHashMap[Item.Position]);
        }
        return 0;
    }
}
