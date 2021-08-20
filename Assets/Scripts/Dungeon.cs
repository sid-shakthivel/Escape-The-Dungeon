using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public GameObject Chest;
    public GameObject Enemy;

    private SpriteRenderer DungeonSpriteRenderer;

    private void Awake()
    {
        DungeonSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Invoke("CreateChest", 0.5f);
        InvokeRepeating("CreateEnemy", 0.5f, 60f);
    }

    private void CreateChest()
    {
        Create(Chest);
    }

    private void CreateEnemy()
    {
        CreateEnemy();
    }

    private void Create(GameObject Object)
    {
        Vector3 DungeonBounds = DungeonSpriteRenderer.bounds.extents;
        Vector2 DungeonPosition = transform.position;
        Vector2 Position = GetRandomPosition(DungeonPosition.x - DungeonBounds.x + 1, DungeonPosition.x + DungeonBounds.x - 1, DungeonPosition.y - DungeonBounds.y + 1, DungeonPosition.y + DungeonBounds.y - 1);
        Instantiate(Object, Position, Quaternion.identity);
    }

    private Vector2 GetRandomPosition(float MinX, float MaxX, float MinY, float MaxY)
    {
        return new Vector2(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
    }
}
