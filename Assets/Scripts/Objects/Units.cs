using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : BaseObject {
    public int damage;
    public enum UNIT_STATE
    {
        IDLE,
        MOVE,
        ATTACK
    }
    protected UNIT_STATE state;

    private void Awake()
    {
        objectType = OBJECT_TYPE.UNITS;
    }

    private void Update()
    {
        switch(state)
        {
            case UNIT_STATE.IDLE:
                Idle();
                break;
            case UNIT_STATE.MOVE:
                Move();
                break;
            case UNIT_STATE.ATTACK:
                Attack();
                break;
        }
    }

    protected virtual void Idle()
    {

    }

    protected virtual void Move()
    {

    }

    protected virtual void Attack()
    {

    }
}
