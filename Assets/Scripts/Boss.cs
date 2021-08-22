using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Dictionary<DungeonNode, List<DungeonNode>> DungeonHashMap;
    private GameObject Player;
    private Vector2 Target;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        DungeonHashMap = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>().DungeonHashMap;
    }

    private void Start()
    {
        Invoke("ChangePosition", 0.5f);
    }

    private void ChangePosition()
    { 
        transform.position = Dungeon.GetRandomDungeon();
        Dijksta(DungeonHashMap.First().Key, DungeonHashMap.First(Node => Node.Key.Position == (Vector2)Player.transform.position).Key);
    }

    private Dictionary<DungeonNode, DungeonNode> Dijksta(DungeonNode Start, DungeonNode End)
    {
        Dictionary<DungeonNode, DungeonNode> Result = new Dictionary<DungeonNode, DungeonNode>();
        SortedDictionary<DungeonNode, float> PriorityQueue = new SortedDictionary<DungeonNode, float>();

        foreach (var Node in DungeonHashMap)
        {
            Node.Key.DjkstraDistance = Int32.MaxValue;
            Result.Add(Node.Key, null);
            PriorityQueue.Add(Node.Key, Int32.MaxValue);
        }
        Start.DjkstraDistance = 0;

        while (PriorityQueue.Count != 0)
        {
            var CurrentNode = PriorityQueue.First();
            if (CurrentNode.Key == End) return Result;

            foreach (var NeighbourNode in DungeonHashMap[CurrentNode.Key])
            {
                float Distance = CurrentNode.Key.DjkstraDistance + NeighbourNode.Cost;
                if (Distance < NeighbourNode.DjkstraDistance)
                {
                    Result[NeighbourNode] = CurrentNode.Key;
                    PriorityQueue.Remove(NeighbourNode);
                    PriorityQueue.Add(NeighbourNode, Distance);
                }
            }
        }
        return null;
    }
}
