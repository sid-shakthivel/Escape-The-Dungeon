using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boss : MonoBehaviour
{
    //public float Speed;

    //private List<DungeonNode> Graph = new List<DungeonNode>();
    //private Dictionary<DungeonNode, DungeonNode> VisitedNodes;
    //private List<DungeonNode> Path = new List<DungeonNode>();
    //private GameObject Player;
    //private DungeonNode BossDungeonNode;
    //private Animator BossAnimator;

    //private int PathIndex = 0;

    //private void Awake()
    //{
    //    Player = GameObject.FindGameObjectWithTag("Player");
    //    Graph = GameObject.FindGameObjectWithTag("GameGenerator").GetComponent<GameGenerator>().Graph;
    //    BossAnimator = GetComponent<Animator>();
    //}

    //private void Start()
    //{
    //    GameObject BossDungeon =  Helper.FindClosestGameObject("Dungeon", transform.position);
    //    GameObject PlayerDungeon = Helper.FindClosestGameObject("Dungeon", Player.transform.position);
    //    BossDungeonNode =  Graph.Find(Node => GameGenerator.GetDungeonPosition(Node.Position) == (Vector2)BossDungeon.transform.position);
    //    DungeonNode PlayerDungeonNode = Graph.Find(Node => GameGenerator.GetDungeonPosition(Node.Position) == (Vector2)PlayerDungeon.transform.position);

    //    VisitedNodes = Dijkstra(BossDungeonNode, PlayerDungeonNode);
    //    SetShortestPath(PlayerDungeonNode);
    //}

    //private void Update()
    //{
    //    if (PathIndex < Path.Count)
    //    {
    //        Vector2 VectorToDungeon = (GameGenerator.GetDungeonPosition(Path[PathIndex].Position) - (Vector2)transform.position).normalized;
    //        BossAnimator.SetFloat("HorizontalSpeed", VectorToDungeon.x);
    //        BossAnimator.SetFloat("VerticalSpeed", VectorToDungeon.y);
    //        transform.position = Vector2.MoveTowards(transform.position, GameGenerator.GetDungeonPosition(Path[PathIndex].Position), Speed * Time.deltaTime);
    //        if ((Vector2)transform.position == GameGenerator.GetDungeonPosition(Path[PathIndex].Position))
    //            PathIndex++;
    //    }
    //}

    //private Dictionary<DungeonNode, DungeonNode> Dijkstra(DungeonNode Start, DungeonNode End)
    //{
    //    Dictionary<DungeonNode, DungeonNode> Result = new Dictionary<DungeonNode, DungeonNode>();
    //    List<DungeonNode> UnvisitedNodes = new List<DungeonNode>();

    //    foreach (DungeonNode Node in Graph)
    //    {
    //        if (Node != Start)
    //            Result.Add(Node, null);
    //        UnvisitedNodes.Add(Node);
    //    }

    //    Start.DjkstraDistance = 0;

    //    while (UnvisitedNodes.Count != 0)
    //    {
    //        UnvisitedNodes.Sort(new DungeonNodeComparer());
    //        DungeonNode CurrentNode = UnvisitedNodes.First();

    //        foreach (DungeonNode NeighbourNode in CurrentNode.Neighbours)
    //        {
    //            if (UnvisitedNodes.Contains(NeighbourNode))
    //            {
    //                int Distance = CurrentNode.DjkstraDistance + NeighbourNode.Cost;
    //                if (Distance < NeighbourNode.DjkstraDistance)
    //                {
    //                    NeighbourNode.DjkstraDistance = Distance;
    //                    Result[NeighbourNode] = CurrentNode;
    //                }
    //            }
    //        }

    //        UnvisitedNodes.Remove(CurrentNode);
    //    }

    //    return Result;
    //}

    //private void SetShortestPath(DungeonNode CurrentNode)
    //{
    //    while (true)
    //    {
    //        Path.Add(CurrentNode);
    //        if (CurrentNode == BossDungeonNode)
    //            break;
    //        CurrentNode = VisitedNodes[CurrentNode];
    //    }
    //    Path.Reverse();
    //}
}
