using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BaseObject : MonoBehaviour {
    public enum OBJECT_TYPE
    {
        RESOURCE,
        BUILDING,
        UNITS,
        MAX,
        NIL
    }

    public int maxHealth;
    public bool isDead = false;
    [HideInInspector]
    public OBJECT_TYPE objectType;
    [SerializeField]
    protected int health;
    [SerializeField]
    protected TileScript currentTile;
    public TileScript CurrentTile
    {
        get
        {
            return currentTile;
        }
    }

    protected virtual void Awake()
    {
        objectType = OBJECT_TYPE.NIL;
        health = maxHealth;
        SubscriptionSystem.Instance.SubscribeEvent("TileUpdated", InitCurrentTile);
    }

    protected virtual void InitCurrentTile()
    {
        SubscriptionSystem.Instance.UnsubscribeEvent("TileUpdated", InitCurrentTile);
        currentTile = TileSystem.Instance.GetTileScript(transform.position);
        currentTile.UnitEnterTile(this.gameObject);
        transform.position = currentTile.transform.position;
        ObjectManager.Instance.AddObject(objectType, this);
    }

    public virtual void TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            isDead = true;
            Destroy(this.gameObject);
        }
    }

    public virtual void RestoreHealth(int amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
    }

    protected virtual void OnDestroy()
    {
        ObjectManager.Instance.RemoveObject(objectType, this);
        currentTile.UnitExitTile(this.gameObject);
    }
}
