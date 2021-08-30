using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameGenerator : MonoBehaviour
{
    public Tilemap FloorTileMap;
    public Tilemap WallTileMap;
    public TileBase FloorTile;
    public TileBase WallTile;

    private int Iterations = 30;
    private int[,] Grid = new int[5, 5];
    private Vector2 OldDungeon;

    private HashSet<Vector2> AllTiles = new HashSet<Vector2>();

    private void Start()
    {
        CreateDungeons();
        CreatePaths();
        CreateWalls(AllTiles);
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
        Grid[(int)DungeonVector.x, (int)DungeonVector.y] = 1;
        HashSet<Vector2> TilePositions = RunRandomWalk(GetDungeonPosition(DungeonVector));
        foreach (Vector2 TilePosition in TilePositions)
            PaintSingleTile(FloorTileMap, FloorTile, TilePosition);
        AllTiles.UnionWith(TilePositions);
    }

    public static Vector2 GetDungeonPosition(Vector2 GridDungeonVector)
    {
        Vector2 GridCentreVector = new Vector2(2, 2);
        Vector2 CentreVector = new Vector2(0, 0);
        return ((GridDungeonVector - GridCentreVector) * 20) + CentreVector;
    }

    private void PaintSingleTile(Tilemap CurrentTileMap, TileBase Tile, Vector2 Position)
    {
        CurrentTileMap.SetTile(CurrentTileMap.WorldToCell(Position), Tile);
    }

    private HashSet<Vector2> RunRandomWalk(Vector2 StartPosition)
    {
        HashSet<Vector2> TilePositions = new HashSet<Vector2>();
        Vector2 CurrentPosition = StartPosition;
        for (int i = 0; i < Iterations; i++)
        {
            TilePositions.UnionWith(RandomWalk(CurrentPosition, 30));
            CurrentPosition = TilePositions.ElementAt(Random.Range(0, TilePositions.Count));
        }
        return TilePositions;
    }

    private HashSet<Vector2> RandomWalk(Vector2 StartPosition, int WalkLength)
    {
        HashSet<Vector2> Path = new HashSet<Vector2>();
        Vector2 CurrentPosition = StartPosition;

        for (int i = 0; i < WalkLength; i++)
        {
            Path.Add(CurrentPosition);
            CurrentPosition = CurrentPosition + GetRandomDirection();
        }

        return Path;
    }

    private void CreateWalls(HashSet<Vector2> FloorPositions)
    {
        foreach (Vector2 FloorPosition in FloorPositions)
        {
            foreach (Vector2 Direction in CardinalDirections)
            {
                Vector2 NeighbourPosition = FloorPosition + Direction;
                if (FloorPositions.Contains(NeighbourPosition) == false)
                    PaintSingleTile(WallTileMap, WallTile, NeighbourPosition);
            }
        }
    }

    private List<Vector2> CardinalDirections = new List<Vector2>() { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };

    private Vector2 GetRandomDirection()
    {
        return CardinalDirections.ElementAt(Random.Range(0, CardinalDirections.Count));
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

    private void CreatePaths()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
                if (Grid[i, j] == 1)
                    for (int k = j + 1; k < 5; k++)
                        if (Grid[i, k] == 1)
                        {
                            CreatePath(new Vector2(i, j), new Vector2(i, k));
                            j = k;
                        }
            for (int j = 0; j < 5; j++)
                if (Grid[j, i] == 1)
                    for (int k = j + 1; k < 5; k++)
                        if (Grid[k, i] == 1)
                        {
                            CreatePath(new Vector2(j, i), new Vector2(k, i));
                            j = k;
                        }
        }
    }

    private void CreatePath(Vector2 Position1, Vector2 Position2)
    {
        Vector2 ScaledPosition1 = GetDungeonPosition(Position1);
        Vector2 ScaledPosition2 = GetDungeonPosition(Position2);

        Vector2 CurrentPosition1 = ScaledPosition1;
        Vector2 CurrentPosition2 = ScaledPosition1 + (ScaledPosition1.x == ScaledPosition2.x ? new Vector2(0, 1) : new Vector2(1, 0));

        while (CurrentPosition1.Equals(ScaledPosition2) == false)
        {
            AllTiles.Add(CurrentPosition1);
            //AllTiles.Add(CurrentPosition2);
            PaintSingleTile(FloorTileMap, FloorTile, CurrentPosition1);
            //PaintSingleTile(FloorTileMap, FloorTile, CurrentPosition2);
            CurrentPosition1 += (ScaledPosition1.x == ScaledPosition2.x ? new Vector2(0, 1) : new Vector2(1, 0));
            //CurrentPosition2 += (ScaledPosition1.x == ScaledPosition2.x ? new Vector2(0, 1) : new Vector2(1, 0));
        }
    }
}