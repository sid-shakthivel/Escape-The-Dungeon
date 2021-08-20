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
        Grid[2, 2] = 1;

        int DungeonCount = 0;
        while (DungeonCount < 7)
        {
            int Option = Random.Range(0, 2);

            Vector2 NewDungeon = GetNewCell(2, 2, 2, 2);
            if (Option == 0 && CheckForHorizontalSpaceInGrid((int)OldDungeon.y))
                NewDungeon = GetNewCell(0, 4, (int)OldDungeon.y, (int)OldDungeon.y);
            else if (Option == 1 && CheckForVerticalSpaceInGrid((int)OldDungeon.x))
                NewDungeon = GetNewCell((int)OldDungeon.x, (int)OldDungeon.x, 0, 4);

            if (Grid[(int)NewDungeon.x, (int)NewDungeon.y] != 1)
            {
                Grid[(int)NewDungeon.x, (int)NewDungeon.y] = 1;
                CreateDungeon(NewDungeon);
                DungeonCount += 1;
            } 
        }
    }

    private void CreateDungeon(Vector2 DungeonVector)
    {
        GameObject NewDungeon = Instantiate(Dungeon, GetDungeonPosition(DungeonVector), Quaternion.identity);
        NewDungeon.transform.SetParent(transform);
        OldDungeon = DungeonVector;
    }

    private void CreatePaths()
    {
        for (int i = 0; i < 5; i++)
        {
            List<int> VerticalCells = new List<int>();
            List<int> HorizontalCells = new List<int>();
            for (int j = 0; j < 5; j++)
            {
                VerticalCells.Add(Grid[i, j]);
                HorizontalCells.Add(Grid[j, i]);
            }
            CreatePath(VerticalCells, i, true);
            CreatePath(HorizontalCells, i, false);
        }
    }

    private void CreatePath(List<int> Array, int GridIndex, bool IsVertical)
    {
        int CurrentDungeonIndex = GetNextFilledElement(Array, 0);
        int LastDungeonIndex;

        if (CurrentDungeonIndex == -1)
            return;
        else
        {
            while (true)
            {
                LastDungeonIndex = CurrentDungeonIndex;
                CurrentDungeonIndex = GetNextFilledElement(Array, LastDungeonIndex + 1);
                if (CurrentDungeonIndex == -1)
                    break;
                else
                {
                    Vector2 LastIndexPostion = GetDungeonPosition(new Vector2(GridIndex, LastDungeonIndex));
                    Vector2 IndexPosition = GetDungeonPosition(new Vector2(GridIndex, CurrentDungeonIndex));

                    Vector2 Position = IsVertical == true ? (IndexPosition + LastIndexPostion) / 2 : InverseVector((IndexPosition + LastIndexPostion) / 2);

                    float Size = IsVertical == true ? ((IndexPosition - LastIndexPostion).y - 10) : ((InverseVector(IndexPosition) - InverseVector(LastIndexPostion)).x - 10);

                    Path.transform.localScale = new Vector3(5f, Size, 0);

                    GameObject NewPath = Instantiate(Path, Position, IsVertical ? Quaternion.identity : Quaternion.Euler(0, 0, 90));
                    NewPath.transform.SetParent(transform);
                }
            }
        }
    }

    private int GetNextFilledElement(List<int> Array, int Index)
    {
        for (int i = Index; i < Array.Count; i++)
        {
            if (Array[i] == 1)
            {
                return i;
            }
        }
        return -1;
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
        {
            if (Grid[i, j] != 1)
                return true;
        }
        return false;
    }

    private bool CheckForVerticalSpaceInGrid(int j)
    {
        for (int i = 0; i < 5; i++)
        {
            if (Grid[j, i] != 1)
                return true;
        }
        return false;
    }

    private void PrintGrid()
    {
        for (int i = 0; i < 5; i++)
        {
            string line = "";
            for (int j = 0; j < 5; j++)
            {
                line += Grid[i, j];
            }
            Debug.Log(line);
        }
    }

    private Vector2 InverseVector(Vector2 Vector)
    {
        return new Vector2(Vector.y, Vector.x);
    }
}
