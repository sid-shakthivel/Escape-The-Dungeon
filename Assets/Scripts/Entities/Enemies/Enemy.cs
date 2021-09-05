using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using EntityNamespace;

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

        protected override void Start()
        {
            PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
            EntitySpeed = 2.5f;
        }

        protected override void Move()
        {
            PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
            MovementVector = (PlayerGameObject.transform.position - transform.position).normalized;
            DistanceToPlayer = Vector2.Distance(PlayerGameObject.transform.position, transform.position);
        }

        protected override IEnumerator OnCollisionEnter2D(Collision2D Collision)
        {
            if (Collision.gameObject.CompareTag("Arrow"))
            {
                EntityHeartCount -= InflictedDamage;
                if (EntityHeartCount <= 0)
                {
                    EntityAnimator.SetBool("IsDead", true);
                    yield return new WaitForSeconds(2);
                    Instantiate(Drops[Random.Range(0, Drops.Count)], transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }
    }
}
