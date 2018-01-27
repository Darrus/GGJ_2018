using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Units : BaseObject {
    public int damage;
    public int moveSpeed;
    public enum UNIT_STATE
    {
        IDLE,
        MOVE,
        ATTACK
    }
    protected UNIT_STATE state;
    protected BaseObject target;
    protected Vector3 targetLastPos;
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
        Vector3Int v3IntStartPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
        currentTile = tilemap.GetInstantiatedObject(v3IntStartPos).GetComponent<TileScript>();
        currentTile.UnitEnterTile(this.gameObject);

        state = UNIT_STATE.MOVE;
        UnityEngine.Vector3Int v3IntEndPos = new Vector3Int((int)pos.x, (int)pos.y, 0);
        path = new Queue<TileScript>(
            TileSystem.Instance.GetFoundPath(
            currentTile,
            tilemap.GetInstantiatedObject(v3IntEndPos).GetComponent<TileScript>()));
    }

    protected virtual void Idle()
    {

    }

    protected virtual void Move()
    {

        Vector3 direction = path.Peek().transform.position - transform.position;
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
