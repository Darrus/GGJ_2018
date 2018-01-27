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

    protected int health;
    protected TileScript currentTile;

    private void Awake()
    {
        objectType = OBJECT_TYPE.NIL;
        health = maxHealth;
    }

    private void Start()
    {
       
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            isDead = true;
            Destroy(this);
        }
    }

    public void RecoverHealth(int amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
    }
}
