using UnityEngine;

public class ResourceObject : BaseObject
{
    public ResourceManager.RESOURCE_TYPE resourceType;

    protected override void Awake()
    {
        base.Awake();
        objectType = OBJECT_TYPE.RESOURCE;
    }

    public override void TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            amount -= health;
            isDead = true;
            ResourceManager.Instance.SetResourceTile(null, transform.position);
        }
    }

    public int Gather(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            amount -= health;
            isDead = true;
            ResourceManager.Instance.SetResourceTile(null, transform.position);
        }
        return amount;
    }
}
