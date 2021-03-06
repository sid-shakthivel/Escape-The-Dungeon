using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using EntityNamespace;
using UnityEngine.Tilemaps;

namespace EnemyNamespace
{
    public class Djkstra
    {
        public Dictionary<Tile, Tile> Result;

        public Djkstra(HashSet<Tile> Graph, Tile StartNode)
        {
            Dictionary<Tile, Tile> VisitedNodes = new Dictionary<Tile, Tile>();
            List<Tile> UnvisitedNodes = new List<Tile>();

            foreach (Tile Node in Graph)
            {
                Node.CostFromStart = int.MaxValue;
                if (Node != StartNode)
                    VisitedNodes.Add(Node, null);
                UnvisitedNodes.Add(Node);
            }

            StartNode.CostFromStart = 0;

            while (UnvisitedNodes.Count != 0)
            {
                UnvisitedNodes.Sort(new TileComparer());
                Tile CurrentTile = UnvisitedNodes.First();

                foreach (Vector3Int Direction in Helper.CardinalDirections)
                {
                    Tile NeighbourTile = null;
                    try
                    {
                        NeighbourTile = GameGenerator.Graph.First(Node => Node.Position == (CurrentTile.Position + Direction));
                        int Distance = CurrentTile.CostFromStart + 1;
                        if (Distance < NeighbourTile.CostFromStart)
                        {
                            NeighbourTile.CostFromStart = Distance;
                            VisitedNodes[NeighbourTile] = CurrentTile;
                        }
                    }
                    catch { }
                }

                UnvisitedNodes.Remove(CurrentTile);
            }

            Result = VisitedNodes;
        }

        public List<Tile> GetShortestPath(Tile CurrentNode, Tile EndNode)
        {
            List<Tile> Path = new List<Tile>();
            while (true)
            {
                Path.Add(CurrentNode);
                if (CurrentNode == EndNode)
                    break;
                CurrentNode = Result[CurrentNode];
            }
            Path.Reverse();
            return Path;
        }
    }

    public class Enemy : Entity
    {
        public List<GameObject> Drops;
        protected GameObject PlayerGameObject;
        protected float InflictedDamage;
        protected float DistanceToPlayer;

        protected Tilemap FloorTilemap;
        protected List<Tile> Path;
        protected int PathIndex;

        protected override void Start()
        {
            PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
            FloorTilemap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Tilemap>();
            EntitySpeed = 2.5f;
        }

        protected override void Move()
        {
            if (Path == null)
                return;

            if (PathIndex < Path.Count)
            {
                Vector3 Target = FloorTilemap.GetCellCenterWorld(FloorTilemap.WorldToCell(Path[PathIndex].Position));
                MovementVector = (Target - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, Target, EntitySpeed * Time.deltaTime);
                if (transform.position == Target)
                    PathIndex++;
            }
        }

        protected virtual IEnumerator GetPath()
        {
            for (; ; )
            {
                Tile CurrentTile = GameGenerator.Graph.First(Node => Node.Position == FloorTilemap.WorldToCell(transform.position));
                Tile PlayerTile = GameGenerator.Graph.First(Node => Node.Position == FloorTilemap.WorldToCell(PlayerGameObject.transform.position));

                Djkstra djkstra = new Djkstra(GameGenerator.Graph, CurrentTile);

                Path = djkstra.GetShortestPath(PlayerTile, CurrentTile);
                PathIndex = 0;

                yield return new WaitForSeconds(2.5f);
            }
        }
    }
}
