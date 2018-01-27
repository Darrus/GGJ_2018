﻿using UnityEngine;

public class Player : Units {
    protected override void Awake()
    {
        base.Awake();
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", Select);
    }

    void Select(GameObject go)
    {
        if(go == this.gameObject)
        {
            Debug.Log(go);
            SubscriptionSystem.Instance.UnsubcribeEvent<GameObject>("LeftClick", Select);
            SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", InteractSelected);
            SubscriptionSystem.Instance.SubscribeEvent<GameObject>("RightClick", Deselect);
        }
    }

    void Deselect(GameObject go)
    {
        SubscriptionSystem.Instance.UnsubcribeEvent<GameObject>("LeftClick", InteractSelected);
        SubscriptionSystem.Instance.UnsubcribeEvent<GameObject>("RightClick", Deselect);
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", Select);
    }

    void InteractSelected(GameObject go)
    {
        BaseObject baseObject = go.GetComponent<BaseObject>();
        if (baseObject != null)
        {
            switch (baseObject.objectType)
            {
                case OBJECT_TYPE.RESOURCE:
                    break;
                case OBJECT_TYPE.UNITS:
                    AttackTarget(baseObject);
                    state = UNIT_STATE.ATTACK;
                    break;
            }
        }
        else
            MoveTo(go.transform.position);
    }

    protected override void Attack()
    {
        Units unit = target as Units;
        if(unit.CurrentTile != unit.lastTile)
        {
            AttackTarget(unit);
            return;
        }
        if(path.Count <=0)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null && actionDelayTimer <= 0.0f)
            {
                Debug.Log(enemy);
                enemy.TakeDamage(damage);
                actionDelayTimer = actionDelayDuration;
            }
            if (target.isDead)
            {
                state = UNIT_STATE.IDLE;
                return;
            }
        }
           

        Vector3 direction = path.Peek().transform.position - transform.position;
        lastTile = currentTile;
        if (direction.sqrMagnitude < 0.01f)
        {
            currentTile.UnitExitTile(this.gameObject);
            currentTile = path.Peek();
            currentTile.UnitEnterTile(this.gameObject);
            path.Dequeue();
            GameObject go = path.Peek().GetOccupyingUnit();
            if(go != null)
            {
                Enemy enemy = go.GetComponent<Enemy>();
                if (enemy != null && enemy == target && actionDelayTimer <= 0.0f)
                {
                    enemy.TakeDamage(damage);
                    actionDelayTimer = actionDelayDuration;
                }
            }
        }

        Vector3 newPosition = transform.position + direction.normalized * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
    }
}
