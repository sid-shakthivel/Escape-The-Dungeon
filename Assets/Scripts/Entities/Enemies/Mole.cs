using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnemyNamespace;

public class Mole : Enemy
{
    public Tilemap DungeonTiles;

    private List<Tile> Path;
    private int PathIndex = 0;

    protected override void Start()
    {
        base.Start();
        EntitySpeed = 1;
        InflictedDamage = 2;

        Tile CurrentTile = GameGenerator.Graph.First(Node => Node.Position == new Vector3((int)transform.position.x, (int)transform.position.y, 0));
        Tile PlayerTile = GameGenerator.Graph.First(Node => Node.Position == new Vector3Int((int)PlayerGameObject.transform.position.x, (int)PlayerGameObject.transform.position.y, (int)PlayerGameObject.transform.position.z));

        Djkstra djkstra = new Djkstra(GameGenerator.Graph, CurrentTile);
        Path = djkstra.GetShortestPath(PlayerTile, CurrentTile);
    }

    protected override void Move()
    {
        if (Path != null && PathIndex < Path.Count)
        {
            MovementVector = (Path[PathIndex].Position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, Path[PathIndex].Position, EntitySpeed * Time.deltaTime);
            if (transform.position == Path[PathIndex].Position)
                PathIndex++;
        }
    }
}
