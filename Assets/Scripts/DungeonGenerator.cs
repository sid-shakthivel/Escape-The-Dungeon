using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject Dungeon;
    public GameObject Path;

    private int[,] Grid = new int[5, 5];
    private Vector2 OldDungeon;

    private void Start()
    {            
        CreateDungeons();
        CreatePaths();
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

    private void CreatePath(Vector2 Position1, Vector2 Position2)
    {
        Position1 = GetDungeonPosition(Position1);
        Position2 = GetDungeonPosition(Position2);
        Vector2 Position = (Position1 + Position2) / 2;

        if (Position1.x == Position2.x)
            Path.transform.localScale = new Vector3(5f, ((Position1 - Position2).y + 10), 0);

        if (Position1.y == Position2.y)
            Path.transform.localScale = new Vector3(5f, ((Position1 - Position2).x + 10), 0);

        Instantiate(Path, Position, Position1.y == Position2.y ? Quaternion.identity : Quaternion.Euler(0, 0, 90));
    }

    private void CreateDungeon(Vector2 DungeonVector)
    {
        GameObject NewDungeon = Instantiate(Dungeon, GetDungeonPosition(DungeonVector), Quaternion.identity);
        NewDungeon.transform.SetParent(transform);
        Grid[(int)DungeonVector.x, (int)DungeonVector.y] = 1;
        OldDungeon = DungeonVector;
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

    private void CreatePaths()
    {
        for (int i = 0; i < 5; i++)
        {
            int k = 1;
            for (int j = 0; j < 5; j++)
            {
                if (Grid[i, j] == 1 && Grid[i, k] == 1)
                    CreatePath(new Vector2(i, j), new Vector2(i, k));
                if (Grid[j, i] == 1 && Grid[k, i] == 1)
                    CreatePath(new Vector2(j, i), new Vector2(k, i));
                k++;
            }
        }
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