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
        StartCoroutine(GetPath());
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

    private IEnumerator GetPath()
    {
        //for (; ;)
        //{
        //    Djkstra djkstra = new Djkstra(GameGenerator.Graph, CurrentTile);
        //    Path = djkstra.GetShortestPath(PlayerTile, CurrentTile);
        //    PathIndex = 0;

        //    foreach (Tile tile in Path)
        //        Debug.Log(tile.Position);

        //    yield return new WaitForSeconds(60);
        //    yield return null;
        //}

        Tile CurrentTile = GameGenerator.Graph.First(Node => Node.Position == DungeonTiles.WorldToCell(transform.position));
        Tile PlayerTile = GameGenerator.Graph.First(Node => Node.Position == DungeonTiles.WorldToCell(PlayerGameObject.transform.position));

        yield return null;
    }
}
