using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingManager : Singleton<BuildingManager> {
    [SerializeField, Tooltip("Tilemap which contains the buildings")]
    protected Tilemap m_BuildingTilemap;

    public void SetBuildingTile(Tile _whatTile, Vector3 _pos)
    {
        int PositionOfTileX_ToTileMap = (int)(_pos.x / m_BuildingTilemap.cellSize.x);
        int PositionOfTileY_ToTileMap = (int)(_pos.y / m_BuildingTilemap.cellSize.y);
        m_BuildingTilemap.SetTile(new Vector3Int(PositionOfTileX_ToTileMap, PositionOfTileY_ToTileMap, 0), _whatTile);
    }

    public void RemoveBuildingTile(Vector3 _pos)
    {
        int PositionOfTileX_ToTileMap = (int)(_pos.x / m_BuildingTilemap.cellSize.x);
        int PositionOfTileY_ToTileMap = (int)(_pos.y / m_BuildingTilemap.cellSize.y);
        m_BuildingTilemap.SetTile(new Vector3Int(PositionOfTileX_ToTileMap, PositionOfTileY_ToTileMap, 0), null);
    }
}
