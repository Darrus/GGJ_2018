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
    [HideInInspector]
    public int health;
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
        Debug.Log(TileSystem.Instance);
        currentTile = TileSystem.Instance.GetTileScript(transform.position);
        currentTile.UnitEnterTile(this.gameObject);
        if(objectType == OBJECT_TYPE.UNITS)
           transform.position = currentTile.transform.position + new Vector3(1.25f, 1.25f);
        else
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
        if(ObjectManager.Instance != null)
            ObjectManager.Instance.RemoveObject(objectType, this);
        if(currentTile != null)
            currentTile.UnitExitTile(this.gameObject);
    }
}
