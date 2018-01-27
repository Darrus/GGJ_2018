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

    protected OBJECT_TYPE objectType;
    public int maxHealth;
    protected int health;
    public bool isDead = false;

    private void Awake()
    {
        objectType = OBJECT_TYPE.BASE;
        health = maxHealth;
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
