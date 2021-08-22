using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonNode
{
    public Vector2 Position;
    public float Distance;

    public DungeonNode(Vector2 DungeonNodePosition, float DungeonNodeDistance)
    {
        Position = DungeonNodePosition;
        Distance = DungeonNodeDistance;
    }
}

public class DungeonGenerator : MonoBehaviour
{
    public GameObject Dungeon;
    public GameObject Path;
    public Dictionary<Vector2, List<DungeonNode>> DungeonHashMap = new Dictionary<Vector2, List<DungeonNode>>();

    private int[,] Grid = new int[5, 5];
    private Vector2 OldDungeon;
    private Vector2 Target;

    private void Start()
    {            
        CreateDungeons();
        CreatePaths();

        foreach (var Item in DungeonHashMap)
        {
            Target = Item.Key;
        }

        float Distance = GetShortestPath(DungeonHashMap.First());
        Debug.Log(Mathf.Abs(Distance));
    }

    private void CreateDungeons()
    {
        CreateDungeon(GetNewCell(2, 2, 2, 2));

        int DungeonCount = 0;
        while (DungeonCount < 5)
        {
            int Option = Random.Range(0, 2);

            Vector2 NewDungeon = GetNewCell(2, 2, 2, 2);
            if (Option == 0 && CheckForHorizontalSpaceInGrid((int)OldDungeon.y))
                NewDungeon = GetNewCell(0, 4, (int)OldDungeon.y, (int)OldDungeon.y);
            else if (Option == 1 && CheckForVerticalSpaceInGrid((int)OldDungeon.x))
                NewDungeon = GetNewCell((int)OldDungeon.x, (int)OldDungeon.x, 0, 4);

            if (Grid[(int)NewDungeon.x, (int)NewDungeon.y] != 1)
            {
                DungeonCount += 1;
                CreateDungeon(NewDungeon);
            }
        }
    }

    private void CreateDungeon(Vector2 DungeonVector)
    {
        GameObject NewDungeon = Instantiate(Dungeon, GetDungeonPosition(DungeonVector), Quaternion.identity);
        NewDungeon.transform.SetParent(transform);
        Grid[(int)DungeonVector.x, (int)DungeonVector.y] = 1;
        DungeonHashMap.Add(DungeonVector, new List<DungeonNode>());
        OldDungeon = DungeonVector;
    }

    private void CreatePaths()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (Grid[i, j] == 1)
                    for (int k = j + 1; k < 5; k++)
                        if (Grid[i, k] == 1)
                        {
                            CreatePath(new Vector2(i, j), new Vector2(i, k));
                            j = k;
                        }
                if (Grid[j, i] == 1)
                    for (int k = j + 1; k < 5; k++)
                        if (Grid[k, i] == 1)
                        {
                            CreatePath(new Vector2(j, i), new Vector2(k, i));
                            j = k;
                        }
            }
        }
    }

    private void CreatePath(Vector2 Position1, Vector2 Position2)
    {
        Vector2 ScaledPosition1 = GetDungeonPosition(Position1);
        Vector2 ScaledPosition2 = GetDungeonPosition(Position2);
        Vector2 ScaledPosition = (ScaledPosition1 + ScaledPosition2) / 2;

        float Size = Position1.x == Position2.x ? ((ScaledPosition1 - ScaledPosition2).y + 10) : ((ScaledPosition1 - ScaledPosition2).x + 10);

        Path.transform.localScale = new Vector3(5f, Size, 0);
        Instantiate(Path, ScaledPosition, ScaledPosition1.y == ScaledPosition2.y ? Quaternion.Euler(0, 0, 90) : Quaternion.identity);

        DungeonHashMap[Position1].Add(new DungeonNode(Position2, Size));
        DungeonHashMap[Position2].Add(new DungeonNode(Position1, Size));
    }

    private float GetShortestPath(KeyValuePair<Vector2, List<DungeonNode>> CurrentDungeon)
    {
        foreach (var Dungeon in CurrentDungeon.Value)
        {
            if (Dungeon.Position == Target)
                return Dungeon.Distance;
            else
            {
                DungeonHashMap[Dungeon.Position] = DungeonHashMap[Dungeon.Position].Where(Entry => Entry.Position != CurrentDungeon.Key).ToList();
                return Dungeon.Distance + GetShortestPath(DungeonHashMap.First(Entry => DungeonHashMap.Comparer.Equals(Entry.Key, Dungeon.Position)));
            }
        }
        return 0;
    }

    private Vector2 GetDungeonPosition(Vector2 GridDungeonVector)
    {
        Vector2 GridCentreVector = new Vector2(2, 2);
        Vector2 CentreVector = new Vector2(0, 0);
        return ((GridDungeonVector - GridCentreVector) * 20) + CentreVector;
    }


    private Vector2 GetNewCell(int MinX, int MaxX, int MinY, int MaxY)
    {
        return new Vector2(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
    }

    private bool CheckForHorizontalSpaceInGrid(int j)
    {
        for (int i = 0; i < 5; i++)
            if (Grid[i, j] != 1)
                return true;
        return false;
    }

    private bool CheckForVerticalSpaceInGrid(int j)
    {
        for (int i = 0; i < 5; i++)
            if (Grid[j, i] != 1)
                return true;
        return false;
    }
}