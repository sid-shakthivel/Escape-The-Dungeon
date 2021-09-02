using UnityEngine;
using UnityEngine.Tilemaps;
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
        InflictedDamage = 2;

        Vector3Int CurrentPosition = DungeonTiles.WorldToCell(transform.position);
        Vector3Int PlayerPosition = DungeonTiles.WorldToCell(PlayerGameObject.transform.position);
        Tile CurrentTile = (Tile)(from Node in GameGenerator.Graph
                                where Node.Position == new Vector2(CurrentPosition.x, CurrentPosition.y)
                                select Node);
        Tile PlayerTile = (Tile)(from Node in GameGenerator.Graph
                                where Node.Position == new Vector2(PlayerPosition.x, PlayerPosition.y)
                                select Node);
        Djkstra djkstra = new Djkstra(GameGenerator.Graph, CurrentTile);
        Path = djkstra.GetShortestPath(CurrentTile, PlayerTile);
    }

    protected override void Move()
    {
        if (PathIndex < Path.Count)
        {
            MovementVector = Path[PathIndex].Position - (Vector2)transform.position;
            EntityRigidbody.MovePosition(MovementVector * EntitySpeed * Time.deltaTime);
            if ((Vector2)transform.position == GameGenerator.GetDungeonPosition(Path[PathIndex].Position))
                PathIndex++;
        }
    }
}
