using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private List<DungeonNode> Graph = new List<DungeonNode>();
    private Dictionary<DungeonNode, DungeonNode> VisitedNodes;
    private List<DungeonNode> Path = new List<DungeonNode>();

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
        VisitedNodes = Dijkstra(Graph.First(), Graph.Last());
    }

    private Dictionary<DungeonNode, DungeonNode> Dijkstra(DungeonNode Start, DungeonNode End)
    {
        Dictionary<DungeonNode, DungeonNode> Result = new Dictionary<DungeonNode, DungeonNode>();
        List<DungeonNode> UnvisitedNodes = new List<DungeonNode>();

        foreach (DungeonNode Node in Graph)
        {
            Result.Add(Node, null);
            UnvisitedNodes.Add(Node);
        }

        Start.DjkstraDistance = 0;

        while (UnvisitedNodes.Count != 0)
        {
            UnvisitedNodes.Sort(new DungeonNodeComparer());
            DungeonNode CurrentNode = UnvisitedNodes.First();

            if (CurrentNode == End)
                return Result;

            foreach (DungeonNode NeighbourNode in CurrentNode.Neighbours)
            {
                if (UnvisitedNodes.Contains(NeighbourNode))
                {
                    int Distance = CurrentNode.DjkstraDistance + NeighbourNode.Cost;
                    if (Distance < NeighbourNode.DjkstraDistance)
                    {
                        NeighbourNode.DjkstraDistance = Distance;
                        Result[NeighbourNode] = CurrentNode;
                    }
                }
            }

            UnvisitedNodes.Remove(CurrentNode);
        }

        return null;
    }

    private DungeonNode GetShortestPath(DungeonNode CurrentNode)
    {
        if (VisitedNodes[CurrentNode] == null)
        {
            Path.Add(CurrentNode);
            return null;
        } else
        {
            Path.Add(CurrentNode);
            return GetShortestPath(VisitedNodes[CurrentNode]);
        }
    }
}
