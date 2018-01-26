using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileScript : MonoBehaviour {
    public enum TILE_TYPE
    {
        WALKABLE,
        INACCESSIBLE,

        TOTAL_TYPE,
    }

    [SerializeField, Tooltip("Tile type of this tile!")]
    protected TILE_TYPE m_TileType;
    [SerializeField, Tooltip("Cost to move to this tile. Used by A* search but can be use for other purpose")]
    protected int m_MoveCost;

    public TILE_TYPE TileType
    {
        get { return m_TileType; }
    }

    public int MoveCost
    {
        get { return m_MoveCost; }
    }
}
