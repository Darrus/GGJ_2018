using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour {
    public enum OBJECT_TYPE
    {
        BASE,
        RESOURCE,
        BUILDING,
        UNITS
    }

    public OBJECT_TYPE objectType;
    protected int health;
    public bool isDead = false;

    private void Awake()
    {
        objectType = OBJECT_TYPE.BASE;
    }

    public int TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            isDead = true;
            Destroy(this);
        }
        return 0;
    }
}
