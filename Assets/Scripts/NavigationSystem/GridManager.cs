using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoSingleton<GridManager>
{
    // Grid name on tile pallete
    public const string TILE_OBSTACLE = "Obstacle";

    [Header("Tile map")]
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TileBase _tileObstacle;
    [SerializeField] private TileBase _tileWalkable;

    
    public AStarPathFinding pathfinding;

    public float CellSize
    {
        get
        {
            return _tilemap.layoutGrid.cellSize.x;
        }
    }
 
    public void Init()
    {
        SetPathFinding();
    }

    public PathNode[,] GetGrid()
    {
        var grid = new PathNode[_tilemap.size.x, _tilemap.size.y];
        var size = _tilemap.size;
        var bounds = _tilemap.localBounds;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                var localPos = GridToLocalPosition(new Vector3Int(x, y));
                var cellPos = _tilemap.LocalToCell(localPos);
                var tile = _tilemap.GetTile(cellPos);

                // Debug.Log($"{x},{y} {(tile != null ? tile.name : "null")}");
                grid[x, y] = new PathNode(x, y, !tile.name.Contains(TILE_OBSTACLE));
            }
        }

        return grid;
    }

    public void SetPathFinding()
    {
        pathfinding = new AStarPathFinding(GetGrid());
    }

    public void SetTile(bool isWalkable, Vector3 target)
    {
        Vector3Int cellPosition = _tilemap.WorldToCell(target);
        _tilemap.SetTile(cellPosition, isWalkable ? _tileWalkable : _tileObstacle);
        _tilemap.RefreshAllTiles();
    }
    
    public TileBase GetTile(Vector3 pos)
    {
        var tpos = _tilemap.WorldToCell(pos);
        return _tilemap.GetTile(tpos);
    }

    public Vector3 GridToLocalPosition(Vector3Int position)
    {
        switch (_tilemap.orientation)
        {
            case Tilemap.Orientation.XZ:
                return _tilemap.localBounds.min + new Vector3(position.x, 0, position.y) * CellSize;
            default: //case Tilemap.Orientation.XY:
                return _tilemap.localBounds.min + new Vector3(position.x, position.y) * CellSize;
        }
    }

    public Vector3Int WorldToGridPosition(Vector3 position)
    {
        var cellPos = _tilemap.WorldToCell(position);
        var localPos = _tilemap.CellToLocal(cellPos);
        var pos = (localPos - _tilemap.localBounds.min) / CellSize;
        switch(_tilemap.orientation)
        {
            case Tilemap.Orientation.XZ:
                return new Vector3Int((int)pos.x, (int)pos.z);
            default : //case Tilemap.Orientation.XY:
                return new Vector3Int((int)pos.x, (int)pos.y);
        }
    }

    public Vector3 GridToWorldPosition(Vector3Int position)
    {
        var localPos = GridToLocalPosition(position);
        switch (_tilemap.orientation)
        {
            case Tilemap.Orientation.XZ:
                return _tilemap.LocalToWorld(localPos) + new Vector3(1, 0, 1) * CellSize * 0.5f;
            default: //case Tilemap.Orientation.XY:
                return _tilemap.LocalToWorld(localPos) + new Vector3(1, 1, 0) * CellSize * 0.5f;
        }
    }

    public void SaveTilemap()
    {
        
    }
}