using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private List<DungeonNode> Graph = new List<DungeonNode>();
    private GameObject Player;
    private Vector2 Target;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Graph = GameObject.FindGameObjectWithTag("DungeonGenerator").GetComponent<DungeonGenerator>().Graph;
    }

    private void Start()
    {
        Invoke("Initialisation", 0.5f);
    }

    private void Initialisation()
    { 
        transform.position = Dungeon.GetRandomDungeon();
        Dictionary<DungeonNode, DungeonNode> DungeonHashMap = Dijkstra(Graph.First(), Graph.Last());
        foreach (KeyValuePair<DungeonNode, DungeonNode> Node in DungeonHashMap)
        {
            Debug.Log(Node.Value);
        }
    }

    private Dictionary<DungeonNode, DungeonNode> Dijkstra(DungeonNode Start, DungeonNode End)
    {
        Dictionary<DungeonNode, DungeonNode> Result = new Dictionary<DungeonNode, DungeonNode>();
        List<DungeonNode> PriorityQueue = new List<DungeonNode>();

        foreach (DungeonNode Node in Graph)
        {
            if (Node != Start)
                Result.Add(Node, null);
            PriorityQueue.Add(Node);
        }

        Start.DjkstraDistance = 0;

        while (PriorityQueue.Count != 0)
        {
            PriorityQueue.Sort(new DungeonNodeComparer());

            DungeonNode CurrentNode = PriorityQueue.First();

            if (CurrentNode == End)
            {
                Debug.Log("OH DEAR");
                return Result;
            }

            foreach (DungeonNode NeighbourNode in CurrentNode.Neighbours)
            {
                int Distance = CurrentNode.DjkstraDistance + NeighbourNode.Cost;
                Debug.Log(CurrentNode.DjkstraDistance);
                Debug.Log(NeighbourNode.Cost);
                if (Distance < NeighbourNode.DjkstraDistance)
                {
                    NeighbourNode.DjkstraDistance = Distance;
                    Result[NeighbourNode] = CurrentNode;
                    PriorityQueue.Remove(NeighbourNode);
                    PriorityQueue.Add(NeighbourNode);
                }
            }
        }
        return null;
    }
}
