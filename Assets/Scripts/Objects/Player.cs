using UnityEngine;

public class Player : Units
{
    protected override void Awake()
    {
        base.Awake();
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", Select);
    }

    void Select(GameObject go)
    {
        if(go == this.gameObject)
        {
            SubscriptionSystem.Instance.TriggerEvent("SelectPlayer");
            SubscriptionSystem.Instance.UnsubscribeEvent<GameObject>("LeftClick", Select);
            SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", InteractSelected);
            SubscriptionSystem.Instance.SubscribeEvent<GameObject>("RightClick", Deselect);
        }
    }

    void Deselect(GameObject go)
    {
        SubscriptionSystem.Instance.UnsubscribeEvent<GameObject>("LeftClick", InteractSelected);
        SubscriptionSystem.Instance.UnsubscribeEvent<GameObject>("RightClick", Deselect);
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
                    Deselect(null);
                    break;
                case OBJECT_TYPE.UNITS:
                    Harvester harvest = baseObject as Harvester;
                    if (harvest != null)
                    {
                        Deselect(null);
                        break;
                    }
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

        if(path == null || path.Count <= 0)
        {
            if(target == null)
            {
                state = UNIT_STATE.IDLE;
                return;
            }

            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (actionDelayTimer <= 0.0f)
                {
                    enemy.TakeDamage(damage);
                    actionDelayTimer = actionDelayDuration;
                }
                return;
            }
            if (target.isDead)
            {
                state = UNIT_STATE.IDLE;
                return;
            }
        }

        Debug.Log(path.Count);
        Vector3 direction = path.Peek().transform.position - transform.position;
        lastTile = currentTile;
        if (direction.sqrMagnitude < 0.01f)
        {
            currentTile.UnitExitTile(this.gameObject);
            currentTile = path.Peek();
            currentTile.UnitEnterTile(this.gameObject);
            path.Dequeue();
            if(path.Count > 0)
            {
                GameObject go = path.Peek().GetOccupyingUnit();
                if (go != null)
                {
                    Enemy enemy = go.GetComponent<Enemy>();
                    if (enemy != null && enemy == target && actionDelayTimer <= 0.0f)
                    {
                        enemy.TakeDamage(damage);
                        actionDelayTimer = actionDelayDuration;
                    }
                }
            }
        }

        Vector3 newPosition = transform.position + direction.normalized * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
    }
}
