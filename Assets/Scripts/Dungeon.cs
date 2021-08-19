using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public GameObject Chest;

    private SpriteRenderer DungeonSpriteRenderer;

    private void Awake()
    {
        DungeonSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Invoke("CreateChest", 0.5f);
    }

    private void CreateChest()
    {
        Vector3 DungeonBounds = DungeonSpriteRenderer.bounds.extents;
        Vector2 DungeonPosition = transform.position;
        Vector2 PositionOfChest = GetRandomPosition(DungeonPosition.x - DungeonBounds.x, DungeonPosition.x + DungeonBounds.x - 1, DungeonPosition.y - DungeonBounds.y, DungeonPosition.y + DungeonBounds.y - 1);
        Instantiate(Chest, PositionOfChest, Quaternion.identity);
    }

    private Vector2 GetRandomPosition(float MinX, float MaxX, float MinY, float MaxY)
    {
        return new Vector2(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
    }
}
