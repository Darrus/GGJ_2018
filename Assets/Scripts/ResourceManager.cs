using UnityEngine;

public class ResourceManager : MonoBehaviour {
    public enum RESOURCE_TYPE
    {
        WOOD,
        STONE,
        MAX
    }

    static int[] resources;

    private void Awake()
    {
        resources = new int[(int)RESOURCE_TYPE.MAX];
    }

    public static void AddResource(RESOURCE_TYPE type, int amount)
    {
        resources[(int)type] += amount;
    }

    public static int GetResourceCount(RESOURCE_TYPE type)
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
