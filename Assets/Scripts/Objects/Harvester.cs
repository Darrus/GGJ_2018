using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester : Units
{
    protected override void Awake()
    {
        base.Awake();
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", Select);
    }

    void Select(GameObject go)
    {
        if (go == this.gameObject)
        {
            SubscriptionSystem.Instance.TriggerEvent<GameObject>("SelectPlayer", this.gameObject);
            SubscriptionSystem.Instance.UnsubscribeEvent<GameObject>("LeftClick", Select);
            SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", InteractSelected);
            SubscriptionSystem.Instance.SubscribeEvent<GameObject>("RightClick", Deselect);
        }
    }

    void Deselect(GameObject go)
    {
        SubscriptionSystem.Instance.TriggerEvent("DeselectPlayer");
        SubscriptionSystem.Instance.UnsubscribeEvent<GameObject>("LeftClick", InteractSelected);
        SubscriptionSystem.Instance.UnsubscribeEvent<GameObject>("RightClick", Deselect);
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", Select);
    }

    void InteractSelected(GameObject go)
    {
        if (go == this.gameObject)
            return;

        BaseObject baseObject = go.GetComponent<BaseObject>();

        if (baseObject != null)
        {
            switch (baseObject.objectType)
            {
                case OBJECT_TYPE.RESOURCE:
                    AttackTarget(baseObject);
                    state = UNIT_STATE.ATTACK;
                    break;
                case OBJECT_TYPE.UNITS:
                    Deselect(null);
                    break;
            }
        }
        else
            MoveTo(go.transform.position);
    }

    protected override void Attack()
    {
        if (path.Count <= 0)
        {
            ResourceObject item = target.GetComponent<ResourceObject>();
            if (item != null && actionDelayTimer <= 0.0f)
            {
                Debug.Log(item);
                ResourceManager.Instance.AddResource(item.resourceType, item.Gather(damage));
                actionDelayTimer = actionDelayDuration;
            }
            if (target.isDead)
            {
                state = UNIT_STATE.IDLE;
            }
            return;
        }

        Vector3 direction = path.Peek().transform.position + new Vector3(1.25f, 1.25f) - transform.position;
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
}
