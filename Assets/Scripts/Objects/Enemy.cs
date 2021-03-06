﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Units
{
    public EnemyHealthUI healthBar;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SubscriptionSystem.Instance.TriggerEvent<Transform>("SpawnArrow", transform);
    }

    protected override void Idle()
    {
        float closestDistance = float.MaxValue;
        BaseObject closestTarget = null;

        List<BaseObject> buildings = ObjectManager.Instance.GetList(OBJECT_TYPE.BUILDING);
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

        foreach(BaseObject building in buildings)
        {
            float sqrDist = (transform.position - building.transform.position).sqrMagnitude;
            if (sqrDist < closestDistance)
            {
                closestTarget = building;
                closestDistance = sqrDist;
            }
        }

        if(closestTarget != null)
            AttackTarget(closestTarget);
    }

    protected override void Attack()
    {
        lastTile = currentTile;

        if (path.Count <= 0)
        {
            if (animator.GetInteger("state") != 0)
                animator.SetInteger("state", 0);


            if (target == null)
            {
                state = UNIT_STATE.IDLE;
                return;
            }

            if (actionDelayTimer <= 0.0f)
            {
                if (animator.GetInteger("state") != 2)
                    animator.SetInteger("state", 2);
                target.TakeDamage(damage);
                actionDelayTimer = actionDelayDuration;
            }

            Buildings building = target as Buildings;
            if (target.isDead || (building != null && building.isRuined))
            {
                state = UNIT_STATE.IDLE;
            }

            return;
        }

        if(animator.GetInteger("state") != 1)
            animator.SetInteger("state", 1);

        Vector3 direction = path.Peek().transform.position + new Vector3(1.25f, 1.25f) - transform.position;
        
        // Set sprite direction
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
        if (direction.x < 0.0f)
            spriteRenderer.flipX = true;
        else if (direction.x > 0.0f)
            spriteRenderer.flipX = false;

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
        healthBar.UpdateHealthBar((float)health, (float)maxHealth);
        if (health < 0)
        {
            SubscriptionSystem.Instance.TriggerEvent("DeselectEnemy");
            isDead = true;
            Destroy(this.gameObject);
        }
    }
}
