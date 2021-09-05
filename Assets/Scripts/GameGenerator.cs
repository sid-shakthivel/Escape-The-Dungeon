using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tile
{
    public int CostFromStart { get; set; }
    public Vector3 Position { get; set; }

    public Tile(Vector3 TilePosition)
    {
        Position = TilePosition;
        CostFromStart = int.MaxValue;
    }
}

public class TileComparer : IComparer<Tile>
{
    public int Compare(Tile x, Tile y)
    {
        return x.CostFromStart - y.CostFromStart;
    }
}

public class GameGenerator : MonoBehaviour
{
    public Tilemap FloorTileMap;
    public Tilemap WallTileMap;
    public TileBase FloorTile;
    public TileBase WallTile;
    public GameObject Crate;
    public GameObject Treant;
    public GameObject Mole;
    public GameObject Key;
    public GameObject Lamp;

    private static int ChestCount { get; set; }

    private int Iterations = 30;
    private int[,] Grid = new int[5, 5];
    private Vector3 OldDungeon;
    private HashSet<Vector3> AllTiles = new HashSet<Vector3>();

    public static HashSet<Tile> Graph = new HashSet<Tile>();
        
    private void Start()
    {
        ChestCount = 0;

        CreateDungeons();
        CreatePaths();
        CreateWalls(AllTiles);
        StartCoroutine(CreateCrates(AllTiles));

        foreach (Vector3 TilePosition in AllTiles)
            Graph.Add(new Tile(FloorTileMap.CellToWorld(new Vector3Int((int)TilePosition.x, (int)TilePosition.y, 0))));

        //Create(Mole, AllTiles);
        Create(Key, AllTiles);
    }

    private void CreateDungeons()
    {
        CreateDungeon(GetNewCell(2, 2, 2, 2));

        int DungeonCount = 0;
        while (DungeonCount < 5)
        {
            int Option = Random.Range(0, 2);

            Vector3 NewDungeon = GetNewCell(2, 2, 2, 2);
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

    private void CreateDungeon(Vector3 DungeonVector)
    {
        Grid[(int)DungeonVector.x, (int)DungeonVector.y] = 1;
        HashSet<Vector3> TilePositions = RunRandomWalk(GetDungeonPosition(DungeonVector));
        foreach (Vector3 TilePosition in TilePositions)
            PaintSingleTile(FloorTileMap, FloorTile, TilePosition);
        AllTiles.UnionWith(TilePositions);

        //Create(Treant, TilePositions);
        Create(Lamp, GetDungeonPosition(DungeonVector));
    }

    private void CreatePaths()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (Grid[i, j] == 1)
                    for (int k = j + 1; k < 5; k++)
                    {
                        if (Grid[i, k] == 1)
                        {
                            CreatePath(new Vector3(i, j, 0), new Vector3(i, k, 0));
                            j = k;
                        }
                    }
            }
                
            for (int j = 0; j < 5; j++)
            {
                if (Grid[j, i] == 1)
                {
                    for (int k = j + 1; k < 5; k++)
                    {
                        if (Grid[k, i] == 1)
                        {
                            CreatePath(new Vector3(j, i, 0), new Vector3(k, i, 0));
                            j = k;
                        }
                    }
                }
            }
        }
    }

    private void CreatePath(Vector3 Position1, Vector3 Position2)
    {
        Vector3 ScaledPosition1 = GetDungeonPosition(Position1);
        Vector3 ScaledPosition2 = GetDungeonPosition(Position2);

        Vector3 CurrentPosition1 = ScaledPosition1;
        Vector3 CurrentPosition2 = ScaledPosition1 + (ScaledPosition1.x == ScaledPosition2.x ? new Vector3(1, 0, 0) : new Vector3(0, 1, 0));

        while (CurrentPosition1.Equals(ScaledPosition2) == false)
        {
            AllTiles.Add(CurrentPosition1);
            AllTiles.Add(CurrentPosition2);
            PaintSingleTile(FloorTileMap, FloorTile, CurrentPosition1);
            PaintSingleTile(FloorTileMap, FloorTile, CurrentPosition2);
            CurrentPosition1 += (ScaledPosition1.x == ScaledPosition2.x ? new Vector3(0, 1, 0) : new Vector3(1, 0, 0));
            CurrentPosition2 += (ScaledPosition1.x == ScaledPosition2.x ? new Vector3(0, 1, 0) : new Vector3(1, 0, 0));
        }
    }

    private void CreateWalls(HashSet<Vector3> FloorPositions)
    {
        foreach (Vector3 FloorPosition in FloorPositions)
        {
            foreach (Vector3 Direction in Helper.CardinalDirections)
            {
                Vector3 NeighbourPosition = FloorPosition + Direction;
                if (FloorPositions.Contains(NeighbourPosition) == false)
                    PaintSingleTile(WallTileMap, WallTile, NeighbourPosition);
            }
        }
    }

    private IEnumerator CreateCrates(HashSet<Vector3> Tiles)
    {
        while (true)
        {
            while (ChestCount < 10)
            {
                Create(Crate, Tiles);
                ChestCount++;
            }
            yield return new WaitForSecondsRealtime(60);
        }
    }

    private void Create(GameObject Object, HashSet<Vector3>Tiles)
    {
        Vector3 RandomTile = Tiles.ElementAt(Random.Range(0, Tiles.Count));
        Vector3 Position = FloorTileMap.GetCellCenterWorld(new Vector3Int((int)RandomTile.x, (int)RandomTile.y, 0));
        GameObject InstaniatedObject = Instantiate(Object, Position, Quaternion.identity);
        InstaniatedObject.transform.SetParent(gameObject.transform);
    }

    private void Create(GameObject Object, Vector3 Tile)
    {
        Vector3 Position = FloorTileMap.GetCellCenterWorld(new Vector3Int((int)Tile.x, (int)Tile.y, 0));
        GameObject InstaniatedObject = Instantiate(Object, Position, Quaternion.identity);
        InstaniatedObject.transform.SetParent(gameObject.transform);
    }

    private HashSet<Vector3> RunRandomWalk(Vector3 StartPosition)
    {
        HashSet<Vector3> TilePositions = new HashSet<Vector3>();
        for (int i = 0; i < Iterations; i++)
            TilePositions.UnionWith(RandomWalk(StartPosition, 30));
        return TilePositions;
    }

    private HashSet<Vector3> RandomWalk(Vector3 StartPosition, int WalkLength)
    {
        HashSet<Vector3> Path = new HashSet<Vector3>();
        Vector3 CurrentPosition = StartPosition;

        for (int i = 0; i < WalkLength; i++)
        {
            Path.Add(CurrentPosition);
            CurrentPosition = CurrentPosition + GetRandomDirection();
        }

        return Path;
    }

    private Vector3 GetNewCell(int MinX, int MaxX, int MinY, int MaxY)
    {
        return new Vector3(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY), 0);
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

    public static Vector3 GetDungeonPosition(Vector3 GridDungeonVector)
    {
        Vector3 GridCentreVector = new Vector3(2, 2, 0);
        Vector3 CentreVector = new Vector3(0, 0, 0);
        return ((GridDungeonVector - GridCentreVector) * 20) + CentreVector;
    }

    private Vector3 GetRandomDirection()
    {
        return Helper.CardinalDirections.ElementAt(Random.Range(0, Helper.CardinalDirections.Count));
    }

    private void PaintSingleTile(Tilemap CurrentTileMap, TileBase Tile, Vector3 Position)
    {
        CurrentTileMap.SetTile(CurrentTileMap.WorldToCell(Position), Tile);
    }
}