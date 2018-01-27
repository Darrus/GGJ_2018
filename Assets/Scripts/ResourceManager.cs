using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceManager : Singleton<ResourceManager> {
    public enum RESOURCE_TYPE
    {
        WOOD,
        STEEL,
        MAX
    }

    int[] resources;
    [SerializeField, Tooltip("Tilemap which contains the buildings")]
    protected Tilemap m_resourceTilemap;

    public void SetResourceTile(Tile _whatTile, Vector3 _pos)
    {
        int PositionOfTileX_ToTileMap = (int)(_pos.x / m_resourceTilemap.cellSize.x);
        int PositionOfTileY_ToTileMap = (int)(_pos.y / m_resourceTilemap.cellSize.y);
        m_resourceTilemap.SetTile(new Vector3Int(PositionOfTileX_ToTileMap, PositionOfTileY_ToTileMap, 0), _whatTile);
    }

    private void Awake()
    {
        resources = new int[(int)RESOURCE_TYPE.MAX];
    }

    public void AddResource(RESOURCE_TYPE type, int amount)
    {
        resources[(int)type] += amount;
    }

    public int GetResourceCount(RESOURCE_TYPE type)
    {
        return resources[(int)type];
    }

    public bool UseResource(RESOURCE_TYPE type, int amount)
    {
        if (resources[(int)type] < amount)
            return false;

        resources[(int)type] -= amount;
        return true;
    }
}
