using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Units : BaseObject {
    public int damage;
    public int moveSpeed;
    public float actionDelayDuration;
    public TileScript lastTile;

    public enum UNIT_STATE
    {
        IDLE,
        MOVE,
        ATTACK
    }

    protected UNIT_STATE state;
    protected BaseObject target;
    protected float actionDelayTimer;
    protected Queue<TileScript> path;

    protected override void Awake()
    {
        base.Awake();
        state = UNIT_STATE.IDLE;
        objectType = OBJECT_TYPE.UNITS;
        actionDelayTimer = actionDelayDuration;
    }

    private void Update()
    {
        actionDelayTimer -= Time.deltaTime;
        switch (state)
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

    public void MoveTo(Vector2 pos)
    {
        Tilemap tilemap = TileSystem.Instance.TileMap;
        state = UNIT_STATE.MOVE;
        path = new Queue<TileScript>(
            TileSystem.Instance.GetFoundPath(
            currentTile,
            TileSystem.Instance.GetTileScript(pos)));
    }

    public void AttackTarget(BaseObject go)
    {
        target = go;
        state = UNIT_STATE.ATTACK;
        path = new Queue<TileScript>(TileSystem.Instance.GetFoundPath(currentTile, go.CurrentTile, false));
    }

    protected virtual void Idle()
    {

    }

    protected virtual void Move()
    {
        Vector3 direction = path.Peek().transform.position - transform.position;
        lastTile = currentTile;
        if(direction.sqrMagnitude < 0.01f)
        {
            currentTile.UnitExitTile(this.gameObject);
            currentTile = path.Peek();
            currentTile.UnitEnterTile(this.gameObject);
            path.Dequeue();
            if (path.Count <= 0)
            {
                state = UNIT_STATE.IDLE;
                return;
            }
        }

        Vector3 newPosition = transform.position + direction.normalized * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
    }

    protected virtual void Attack()
    {
        
    }
}
