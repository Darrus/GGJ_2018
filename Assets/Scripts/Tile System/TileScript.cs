using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TileScript : MonoBehaviour {
    public enum TILE_TYPE
    {
        WALKABLE,
        INACCESSIBLE,
        OCCUPIED,

        TOTAL_TYPE,
    }
    [Header("Variables required")]
    [SerializeField, Tooltip("Tile type of this tile!")]
    protected TILE_TYPE m_TileType;
    [Header("Debugging purpose")]
    public int m_NumberOfUnitOccupied = 0;
    [SerializeField, Tooltip("All the units that are currently occupying the tile")]
    public List<GameObject> m_ListOfUnitsOccupying = new List<GameObject>();

    public TILE_TYPE TileType
    {
        get { return m_TileType; }
    }

    public void UnitEnterTile(GameObject _unitGO)
    {
        if (!m_ListOfUnitsOccupying.Contains(_unitGO))
        {
            m_ListOfUnitsOccupying.Add(_unitGO);
            IncrementUnitOccupied();
        }
    }

    public void UnitExitTile(GameObject _unitGO)
    {
        m_ListOfUnitsOccupying.Remove(_unitGO);
        DecrementUnitOccupied();
    }

    public GameObject GetOccupyingUnit()
    {
        return m_ListOfUnitsOccupying.Count > 0 ? m_ListOfUnitsOccupying[0] : null;
    }

    public void IncrementUnitOccupied()
    {
        ++m_NumberOfUnitOccupied;
        m_TileType = TILE_TYPE.OCCUPIED;
    }

    public void DecrementUnitOccupied()
    {
        --m_NumberOfUnitOccupied;
        Assert.IsFalse(m_NumberOfUnitOccupied < 0, "Number of units occupying has goes less than 0");
        if (m_NumberOfUnitOccupied <= 0)
        {
            m_NumberOfUnitOccupied = 0;
            m_TileType = TILE_TYPE.WALKABLE;
        }
    }
}
