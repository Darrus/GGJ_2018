﻿using UnityEngine;

public class Player : Units
{
    public int repairCost = 50;

    protected override void Awake()
    {
        base.Awake();
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", Select);
    }

    void Select(GameObject go)
    {
        if(go == this.gameObject)
        {
            SubscriptionSystem.Instance.TriggerEvent<GameObject>("SelectPlayer", this.gameObject);
            SubscriptionSystem.Instance.SubscribeEvent("Repair", Repair);
            SubscriptionSystem.Instance.UnsubscribeEvent<GameObject>("LeftClick", Select);
            SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", InteractSelected);
            SubscriptionSystem.Instance.SubscribeEvent<GameObject>("RightClick", Deselect);
        }
    }

    void Deselect(GameObject go)
    {
        SubscriptionSystem.Instance.UnsubscribeEvent("Repair", Repair);
        SubscriptionSystem.Instance.UnsubscribeEvent<GameObject>("LeftClick", InteractSelected);
        SubscriptionSystem.Instance.UnsubscribeEvent<GameObject>("RightClick", Deselect);
        SubscriptionSystem.Instance.SubscribeEvent<GameObject>("LeftClick", Select);
    }

    void Repair()
    {
        if (ResourceManager.Instance.GetResourceCount(ResourceManager.RESOURCE_TYPE.STEEL) > repairCost)
        {
            SoundManager.Instance.PlaySFX("NomadStrategyRobot Repair");
            ResourceManager.Instance.UseResource(ResourceManager.RESOURCE_TYPE.WOOD, repairCost);
            health = maxHealth;
            SubscriptionSystem.Instance.UnsubscribeEvent("Repair", Repair);
        }
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
                case OBJECT_TYPE.UNITS:
                    Harvester harvest = baseObject as Harvester;
                    if (harvest != null)
                    {
                        SubscriptionSystem.Instance.TriggerEvent("DeselectEnemy");
                        Deselect(null);
                        break;
                    }
                    SubscriptionSystem.Instance.TriggerEvent<GameObject>("SelectEnemy", baseObject.gameObject);
                    AttackTarget(baseObject);
                    state = UNIT_STATE.ATTACK;
                    break;
            }
        }
        else
        {
            if(target != null)
                SubscriptionSystem.Instance.TriggerEvent("DeselectEnemy");

            MoveTo(go.transform.position);
        }
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
                if (animator.GetInteger("state") != 0)
                    animator.SetInteger("state", 0);
                return;
            }

            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (actionDelayTimer <= 0.0f)
                {
                    if (animator.GetInteger("state") != 2)
                        animator.SetInteger("state", 2);
                    enemy.TakeDamage(damage);
                    actionDelayTimer = actionDelayDuration;
                }
                return;
            }
            if (target.isDead)
            {
                state = UNIT_STATE.IDLE;
                if (animator.GetInteger("state") != 0)
                    animator.SetInteger("state", 0);
                return;
            }
        }

        Vector3 direction = path.Peek().transform.position + new Vector3(1.25f, 1.25f) - transform.position;
        if (animator.GetInteger("state") != 1)
            animator.SetInteger("state", 1);

        // Set sprite direction
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
        if (direction.x < 0.0f)
            spriteRenderer.flipX = true;
        else if (direction.x > 0.0f)
            spriteRenderer.flipX = false;

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

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SubscriptionSystem.Instance.TriggerEvent<BaseObject.OBJECT_TYPE>("UnitDestroyed", objectType);
    }
}
