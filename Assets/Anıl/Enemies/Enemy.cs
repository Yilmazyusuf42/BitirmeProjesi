using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    protected Transform player;
    public Rigidbody2D rb;
    public Animator anim;
    public EnemyStateMachine stateMachine;
    public EntityFx fx { get; private set; }  // ✅ Flash FX handler

    [Header("Movement Settings")]
    public float runSpeed = 3f;
    public float walkSpeed = 1.5f;
    public int facingDir = 1;

    [Header("Aggro Settings")]
    public float minAgroRange = 1.5f;
    public float maxAgroRange = 5f;

    [Header("Idle Settings")]
    public float idleTime = 1.5f;

    [Header("Combat Settings")]
    public bool canBeStunned = true;

    [HideInInspector] 
    public bool returningToSpawn = false;

    [Header("Patrol Settings")]
    public float patrolRange = 3f;
    public Vector2 spawnPosition;

    public virtual float stunDuration => 0.5f;

    // FSM State Hooks
    public virtual EnemyState patrolState => null;
    public virtual EnemyState battleState => null;
    public virtual EnemyState attackState => null;
    public virtual EnemyState stunnedState => null;
    public virtual EnemyState idle => null;

    public virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        fx = GetComponent<EntityFx>();
        stateMachine = new EnemyStateMachine();
        

        Debug.Log($"[Enemy BASE] Awake for {gameObject.name}");
    }

    public virtual void Start()
    {
        spawnPosition = transform.position;
    }

    public virtual void Update()
    {
        stateMachine?.currentState?.Update();
    }

    public void SetVelocity(float x, float y)
    {
        rb.velocity = new Vector2(x, y);
    }

    public void SetZeroVelocity()
    {
        rb.velocity = Vector2.zero;
    }

    public void Flip()
    {
        facingDir *= -1;
        transform.localScale = new Vector3(facingDir, 1, 1);
    }

    public void FlipTowardsPlayer()
    {
        if (player == null) return;

        float dir = player.position.x - transform.position.x;
        facingDir = dir > 0 ? 1 : -1;
        transform.localScale = new Vector3(facingDir, 1, 1);
    }

    public virtual bool IsPlayerDetected()
    {
        return Vector2.Distance(transform.position, player.position) <= maxAgroRange;
    }

    public virtual bool IsPlayerInMinAgroRange()
    {
        return Vector2.Distance(transform.position, player.position) <= minAgroRange;
    }

    public virtual bool IsWallDetected() => false;
    public virtual bool IsGroundDetected() => true;

    public virtual bool CanBeStunned() => canBeStunned;

    public virtual void TakeDamage(int amount)
    {
        if (IsStunned) return;
        
        fx?.Flash();

        if (CanBeStunned() && stunnedState != null)
        {
            stateMachine.ChangeState(stunnedState);
        }

        Debug.Log($"{gameObject.name} took {amount} damage.");
    }

    public virtual void Die()
    {
        anim.SetTrigger("Dead");
        SetZeroVelocity();
        // Optional: disable collider, script, drop loot, etc.
    }

    protected virtual void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.yellow;

        Vector2 leftEdge = Application.isPlaying ? spawnPosition - Vector2.right * patrolRange : transform.position - Vector3.right * patrolRange;
        Vector2 rightEdge = Application.isPlaying ? spawnPosition + Vector2.right * patrolRange : transform.position + Vector3.right * patrolRange;

        Gizmos.DrawLine(leftEdge + Vector2.up * 0.1f, rightEdge + Vector2.up * 0.1f);
        Gizmos.DrawSphere(leftEdge, 0.1f);
        Gizmos.DrawSphere(rightEdge, 0.1f);
#endif
    }
    public bool IsStunned => stateMachine.currentState == stunnedState;
}
