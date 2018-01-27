using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Units
{
    protected override void Idle()
    {
        float closestDistance = float.MaxValue;
        BaseObject closestTarget = null;

        //List<BaseObject> buildings = ObjectManager.Instance.GetList(OBJECT_TYPE.BUILDINGS);
        List<BaseObject> resources = ObjectManager.Instance.GetList(OBJECT_TYPE.RESOURCE);

        foreach(BaseObject resource in resources)
        {
            float sqrDist = (transform.position - resource.transform.position).sqrMagnitude;
            if(sqrDist < closestDistance)
            {
                closestTarget = resource;
                closestDistance = sqrDist;
            }
        }

        if(closestTarget != null)
            AttackTarget(closestTarget);
    }

    protected override void Attack()
    {
        if(path.Count <= 0)
        {
            if(target == null)
            {
                state = UNIT_STATE.IDLE;
                return;
            }

            if (actionDelayTimer <= 0.0f)
            {
                target.TakeDamage(damage);
                actionDelayTimer = actionDelayDuration;
            }

            if (target.isDead)
            {
                state = UNIT_STATE.IDLE;
            }
            return;
        }

        Vector3 direction = path.Peek().transform.position - transform.position;
        lastTile = currentTile;
        if (direction.sqrMagnitude < 0.01f)
        {
            currentTile.UnitExitTile(this.gameObject);
            currentTile = path.Peek();
            currentTile.UnitEnterTile(this.gameObject);
            path.Dequeue();
        }

        Vector3 newPosition = transform.position + direction.normalized * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
    }

    public override void TakeDamage(int amount)
    {
        List<BaseObject> units = ObjectManager.Instance.GetList(OBJECT_TYPE.UNITS);

        foreach(var unit in units)
        {
            if(unit.name == "Player")
            {
                AttackTarget(unit);
            }
        }

        health -= amount;
        if (health < 0)
        {
            isDead = true;
            Destroy(this.gameObject);
        }
    }
}
