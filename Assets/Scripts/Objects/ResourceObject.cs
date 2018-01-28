using UnityEngine;

public class ResourceObject : BaseObject
{
    public ResourceManager.RESOURCE_TYPE resourceType;
    public int maxGatherAmount;
    public int gatherGain;
    int gatherLeft;

    [SerializeField]
    ResourceUI ui;

    protected override void Awake()
    {
        base.Awake();
        objectType = OBJECT_TYPE.RESOURCE;
        gatherLeft = maxGatherAmount;
        if(ui != null)
            ui.UpdateGather(gatherLeft, maxGatherAmount);
    }

    public override void TakeDamage(int amount)
    {
        health -= amount;
        if(ui != null)
            ui.UpdateHealthBar(health, maxHealth);
        if (health < 0)
        {
            amount -= health;
            isDead = true;
            ResourceManager.Instance.SetResourceTile(null, transform.position);
        }
    }

    public int Gather()
    {
        gatherLeft--;
        if(ui != null)
            ui.UpdateGather(gatherLeft, maxGatherAmount);

        if (gatherLeft <= 0)
        {
            isDead = true;
            ResourceManager.Instance.SetResourceTile(null, transform.position);
        }

        return gatherGain;
    }
}
