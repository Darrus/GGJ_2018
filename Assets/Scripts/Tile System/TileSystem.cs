using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Used for A* search
/// </summary>
public class TileSystem : Singleton<TileSystem> {
    [SerializeField, Tooltip("Tile map to walk through")]
    protected Tilemap m_TileMap;
    public Tilemap TileMap
    {
        get { return m_TileMap; }
    }


    [Header("Debugging")]
    [SerializeField, Tooltip("Array of the TileScripts")]
    protected TileScript[] m_WalkableTiles;
    [SerializeField] List<TileScript> m_SearchedPath = new List<TileScript>();

    protected class AStar_Node
    {
        public AStar_Node(TileScript _tileScript, float _gCost, float _hCost, AStar_Node _parent)
        {
            m_TileScript = _tileScript;
            g_cost = _gCost;
            h_cost = _hCost;
            m_Parent = _parent;
        }

        public TileScript m_TileScript;
        public float g_cost; // From start to here.
        public float h_cost; // From here to the end.
        public float f_cost
        {
            get { return g_cost + h_cost; }
        }
        public AStar_Node m_Parent;
    }

	// Use this for initialization
	IEnumerator Start () {
        // delay by a frame so that the tiles will be instantiated
        yield return null;
        m_WalkableTiles = m_TileMap.GetComponentsInChildren<TileScript>();
        SubscriptionSystem.Instance.TriggerEvent("TileUpdated");
        yield break;
    }

    public List<TileScript> GetNeighbourTiles(TileScript _tile)
    {
        List<TileScript> returnList = new List<TileScript>();
        int PositionOfTileX_ToTileMap = (int)(_tile.transform.position.x / m_TileMap.cellSize.x);
        int PositionOfTileY_ToTileMap = (int)(_tile.transform.position.y / m_TileMap.cellSize.y);
        // base on the tile position and grid's tile size!
        for (int x = -1; x <= 1; x += 1)
        {
            for (int y = -1; y <= 1; y += 1)
            {
                // making sure centers and corners arent allowed
                if (x == 0 && y == 0 || (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1))
                    continue;
                int TotalXPosition = x + PositionOfTileX_ToTileMap;
                int TotalYPosition = y + PositionOfTileY_ToTileMap;
                if (!m_TileMap.HasTile(new Vector3Int(TotalXPosition, TotalYPosition, 0)))
                    continue;
                GameObject neighbourTileGO = m_TileMap.GetInstantiatedObject(new Vector3Int(TotalXPosition, TotalYPosition, 0));
                if (neighbourTileGO)
                {
                    returnList.Add(neighbourTileGO.GetComponent<TileScript>());
                }
            }
        }
        return returnList;
    }

    public TileScript GetTileScript(Vector3 pos)
    {
        int PositionOfTileX_ToTileMap = (int)(pos.x / m_TileMap.cellSize.x);
        int PositionOfTileY_ToTileMap = (int)(pos.y / m_TileMap.cellSize.y);
        return m_TileMap.GetInstantiatedObject(new Vector3Int(PositionOfTileX_ToTileMap, PositionOfTileY_ToTileMap, 0)).GetComponent<TileScript>();
    }

    public List<TileScript> GetFoundPath(TileScript _startTile, TileScript _endTile)
    {
        m_SearchedPath.Clear();
        AStar_Node endNodePath = null;
        List<AStar_Node> openSet = new List<AStar_Node>();
        List<TileScript> openSetTileScript = new List<TileScript>();
        HashSet<TileScript> closedSet = new HashSet<TileScript>();
        AStar_Node startPathNode = new AStar_Node(_startTile, 0, Vector3.Distance(_startTile.transform.position, _endTile.transform.position), null);
        openSet.Add(startPathNode);
        openSetTileScript.Add(_startTile);
        while (openSet.Count > 0)
        {
            // get cheapest path
            AStar_Node cheapestNode = null;
            foreach (AStar_Node node in openSet)
            {
                if (cheapestNode == null || cheapestNode.f_cost > node.f_cost)
                {
                    cheapestNode = node;
                }
            }
            if (cheapestNode.m_TileScript == _endTile)
            {
                endNodePath = cheapestNode;
                break;
            }

            // then begin the searching of the neighbours!
            List<TileScript> neighbourOfCheapNode = GetNeighbourTiles(cheapestNode.m_TileScript);
            foreach (TileScript neighbourTile in neighbourOfCheapNode)
            {
                // make sure it is not in the open set and it is walkable
                if (neighbourTile.TileType == TileScript.TILE_TYPE.WALKABLE && !openSetTileScript.Contains(neighbourTile) && !closedSet.Contains(neighbourTile))
                {
                    // this might not be the correct way to calculate GCost but it works for now
                    float GCost = Vector3.Distance(_startTile.transform.position, neighbourTile.transform.position) + cheapestNode.g_cost;
                    float HCost = Vector3.Distance(neighbourTile.transform.position, _endTile.transform.position);
                    AStar_Node nodePath = new AStar_Node(neighbourTile, GCost, HCost, cheapestNode);
                    // then we can add it in the open list
                    openSetTileScript.Add(neighbourTile);
                    openSet.Add(nodePath);
                }
            }
            closedSet.Add(cheapestNode.m_TileScript);
            openSet.Remove(cheapestNode);
            openSetTileScript.Remove(cheapestNode.m_TileScript);
        }
        // then we can start retracing the path
        if (endNodePath != null)
        {
            AStar_Node startTracingNode = endNodePath;
            while (startTracingNode != null)
            {
                m_SearchedPath.Add(startTracingNode.m_TileScript);
                startTracingNode = startTracingNode.m_Parent;
            }
            // reverse since it is from end to start!
            m_SearchedPath.Reverse();
        }
        return m_SearchedPath;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        foreach (TileScript TileDataStuff in m_WalkableTiles)
        {
            if (m_SearchedPath.Contains(TileDataStuff))
            {
                Gizmos.color = Color.blue;
            }
            else
            {
                switch (TileDataStuff.TileType)
                {
                    case TileScript.TILE_TYPE.INACCESSIBLE:
                    case TileScript.TILE_TYPE.OCCUPIED:
                        Gizmos.color = Color.red;
                        break;
                    default:
                        // then it should be normally walkable
                        Gizmos.color = Color.white;
                        break;
                }
            }
            Gizmos.DrawCube(TileDataStuff.transform.position, TileDataStuff.transform.localScale - new Vector3(0.1f,0.1f,0.1f));
        }
    }
#endif
}
