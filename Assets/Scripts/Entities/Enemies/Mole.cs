using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnemyNamespace;

public class Mole : Enemy
{
    Tilemap FloorTilemap;
    private List<Tile> Path;
    private int PathIndex = 0;

    protected override void Start()
    {
        base.Start();
        EntityHeartCount = 20;
        EntitySpeed = 5;
        InflictedDamage = 2;
        FloorTilemap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Tilemap>();
        StartCoroutine(GetPath());
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
        } else
        {
            base.Move();
            EntityRigidbody.velocity = MovementVector * EntitySpeed;
        }
            
    }

    private IEnumerator GetPath()
    {
        for (; ; )
        {
            Tile CurrentTile = GameGenerator.Graph.First(Node => Node.Position == FloorTilemap.WorldToCell(transform.position));
            Tile PlayerTile = GameGenerator.Graph.First(Node => Node.Position == FloorTilemap.WorldToCell(PlayerGameObject.transform.position));

            Djkstra djkstra = new Djkstra(GameGenerator.Graph, CurrentTile);
            Path = djkstra.GetShortestPath(PlayerTile, CurrentTile);
            PathIndex = 0;

            yield return new WaitForSeconds(60);
        }
    }
}
