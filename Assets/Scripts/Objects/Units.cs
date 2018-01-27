using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Units : BaseObject {
    public int damage;
    public int moveSpeed;
    public float actionDelayDuration;
    public TileScript lastTile;
    public BaseObject target;

    public enum UNIT_STATE
    {
        IDLE,
        MOVE,
        ATTACK
    }

    protected UNIT_STATE state;
    protected float actionDelayTimer;
    protected Queue<TileScript> path;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if(path == null || path.Count <= 0)
        {
            state = UNIT_STATE.IDLE;
            return;
        }

        Vector3 direction = (path.Peek().transform.position + new Vector3(1.25f, 1.25f)) - transform.position;
        lastTile = currentTile;

        // Set sprite direction
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
        if (direction.x < 0.0f)
            spriteRenderer.flipX = true;
        else if(direction.x > 0.0f)
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

    protected virtual void Attack()
    {
        
    }
}
