using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Units : BaseObject {
    public int damage;
    public int moveSpeed;
    public float actionDelayTimer;
    public enum UNIT_STATE
    {
        IDLE,
        MOVE,
        ATTACK
    }
    protected UNIT_STATE state;
    protected BaseObject target;
    public TileScript lastTile;
    Queue<TileScript> path;

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

    public void MoveTo(Vector2 pos)
    {
        Tilemap tilemap = TileSystem.Instance.TileMap;
        state = UNIT_STATE.MOVE;
        UnityEngine.Vector3Int v3IntEndPos = new Vector3Int((int)pos.x, (int)pos.y, 0);
        path = new Queue<TileScript>(
            TileSystem.Instance.GetFoundPath(
            currentTile,
            tilemap.GetInstantiatedObject(v3IntEndPos).GetComponent<TileScript>()));
    }

    public void AttackTarget(BaseObject go)
    {
        state = UNIT_STATE.ATTACK;
        path = new Queue<TileScript>(TileSystem.Instance.GetFoundPath(currentTile, go.CurrentTile));
    }

    protected virtual void Idle()
    {

    }

    protected virtual void Move()
    {
        Vector3 direction = path.Peek().transform.position - transform.position;
        if(direction.sqrMagnitude < 0.01f)
        {
            lastTile = currentTile;
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
        Vector3 direction = path.Peek().transform.position - transform.position;
        if (direction.sqrMagnitude < 0.01f)
        {
            lastTile = currentTile;
            currentTile.UnitExitTile(this.gameObject);
            currentTile = path.Peek();
            currentTile.UnitEnterTile(this.gameObject);
            path.Dequeue();
            //if ()
            //{
            //    return;
            //}
        }

        Vector3 newPosition = transform.position + direction.normalized * moveSpeed * Time.deltaTime;
        transform.position = newPosition;
    }
}
