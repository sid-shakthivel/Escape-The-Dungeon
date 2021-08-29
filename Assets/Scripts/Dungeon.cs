using UnityEngine;

public class Dungeon : MonoBehaviour
{
    public GameObject Crate;
    public GameObject Enemy;

    private SpriteRenderer DungeonSpriteRenderer;
    private int EnemyCount;

    private void Awake()
    {
        DungeonSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Invoke("CreateCrate", 0.5f);
        //InvokeRepeating("CreateEnemy", 0.5f, 120f);
    }

    private void CreateCrate()
    {
        Create(Crate);
    }

    private void CreateEnemy()
    {
        if (EnemyCount < 10)
        {
            Create(Enemy);
            EnemyCount++;
        }
    }

    private void Create(GameObject Object)
    {
        Vector3 DungeonBounds = DungeonSpriteRenderer.bounds.extents;
        Vector2 DungeonPosition = transform.position;
        Vector2 Position = GetRandomPosition(DungeonPosition.x - DungeonBounds.x + 1, DungeonPosition.x + DungeonBounds.x - 1, DungeonPosition.y - DungeonBounds.y + 1, DungeonPosition.y + DungeonBounds.y - 1);
        GameObject InstaniatedObject = Instantiate(Object, Position, Quaternion.identity);
        InstaniatedObject.transform.SetParent(gameObject.transform);
    }

    private Vector2 GetRandomPosition(float MinX, float MaxX, float MinY, float MaxY)
    {
        return new Vector2(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
    }

    public static Vector2 GetRandomDungeon()
    {
        GameObject[] DungeonArray = GameObject.FindGameObjectsWithTag("Dungeon");
        return DungeonArray[Random.Range(0, DungeonArray.Length)].transform.position;
    }
}
