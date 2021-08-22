using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonNode 
{
    public Vector2 Position;
    public int Cost;
    public int DjkstraDistance = int.MaxValue;
    public List<DungeonNode> Neighbours = new List<DungeonNode>();

    public DungeonNode(Vector2 DungeonNodePosition)
    {
        Position = DungeonNodePosition;
    }

    public void Set(int Distance, DungeonNode NeighbourNode)
    {
        Cost = Distance;
        Neighbours.Add(NeighbourNode);
    }
}

public class DungeonNodeComparer: IComparer<DungeonNode>
{
    public int Compare(DungeonNode Node1, DungeonNode Node2)
    {
        return Node1.DjkstraDistance - Node2.DjkstraDistance;
    }
}

public class DungeonGenerator : MonoBehaviour
{
    public GameObject Dungeon;
    public GameObject Path;
    public List<DungeonNode> Graph = new List<DungeonNode>();

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

    private void CreateDungeon(Vector2 DungeonVector)
    {
        GameObject NewDungeon = Instantiate(Dungeon, GetDungeonPosition(DungeonVector), Quaternion.identity);
        NewDungeon.transform.SetParent(transform);
        Grid[(int)DungeonVector.x, (int)DungeonVector.y] = 1;
        Graph.Add(new DungeonNode(DungeonVector));
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

        float Size = Position1.x == Position2.x ? Mathf.Abs(((ScaledPosition1 - ScaledPosition2).y + 10)) : Mathf.Abs(((ScaledPosition1 - ScaledPosition2).x + 10));

        Path.transform.localScale = new Vector3(5f, Size, 0);
        GameObject NewPath = Instantiate(Path, ScaledPosition, ScaledPosition1.y == ScaledPosition2.y ? Quaternion.Euler(0, 0, 90) : Quaternion.identity);
        NewPath.transform.SetParent(gameObject.transform);

        DungeonNode Node1 = Graph.Find(Node => Node.Position == Position1);
        DungeonNode Node2 = Graph.Find(Node => Node.Position == Position2);

        Node2.Set((int)Size, Node1);
        Node1.Set((int)Size, Node2);
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