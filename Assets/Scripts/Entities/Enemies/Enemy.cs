using System.Collections;
using UnityEngine;
using EntityNamespace;

namespace EnemyNamespace
{
    using System.Collections.Generic;
    using System.Linq;

    public class Djkstra
    {
        protected Dictionary<Tile, Tile> Result;

        public Djkstra(List<Tile> Graph, Tile StartNode)
        {
            Dictionary<Tile, Tile> VisitedNodes = new Dictionary<Tile, Tile>();
            List<Tile> UnvisitedNodes = new List<Tile>();

            foreach (Tile Node in Graph)
            {
                if (Node != StartNode)
                    VisitedNodes.Add(Node, null);
                UnvisitedNodes.Add(Node);
            }

            StartNode.CostFromStart = 0;

            while (UnvisitedNodes.Count > 0)
            {
                UnvisitedNodes.Sort(new TileComparer());
                Tile CurrentTile = UnvisitedNodes.First();

                foreach (Vector2 Direction in Helper.CardinalDirections)
                {
                    Tile NeighbourTile = (Tile)(from Node in UnvisitedNodes
                                         where Node.Position == (CurrentTile.Position + Direction)
                                         select Node);

                    if (NeighbourTile != null)
                    {
                        int Distance = CurrentTile.CostFromStart + 1;
                        if (Distance < NeighbourTile.CostFromStart)
                        {
                            NeighbourTile.CostFromStart = Distance;
                            VisitedNodes[NeighbourTile] = CurrentTile;
                        }
                    }
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
            //PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
            MovementVector = (PlayerGameObject.transform.position - transform.position).normalized;
            DistanceToPlayer = Vector2.Distance(PlayerGameObject.transform.position, transform.position);

            if (DistanceToPlayer <= 5)
            {
                MovementVector = Vector2.zero;
                EntityRigidbody.velocity = MovementVector;
            }
            else if (DistanceToPlayer <= 15)
                EntityRigidbody.velocity = MovementVector * EntitySpeed;
        }

        protected override IEnumerator OnCollisionEnter2D(Collision2D Collision)
        {
            if (Collision.gameObject.CompareTag("Arrow"))
            {
                EntityHeartCount -= InflictedDamage;
                Destroy(Collision.gameObject);
                if (EntityHeartCount <= 0)
                {
                    EntityAnimator.SetBool("IsDead", true);
                    yield return new WaitForSeconds(2);
                    Destroy(gameObject);
                }
            }
        }
    }
}
