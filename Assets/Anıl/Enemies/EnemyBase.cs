using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour
{
    public Transform player { get; protected set; }

    [Header("References")]
    public Rigidbody2D rb;
    public Animator anim;
    public EnemyStateMachine stateMachine;
    public CharacterStats stats { get; private set; }
    public EntityFx fx { get; private set; }
    public CapsuleCollider2D cd { get; private set; }

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

    [HideInInspector] public bool returningToSpawn = false;

    [Header("Patrol Settings")]
    public float patrolRange = 3f;
    public Vector2 spawnPosition;

    [Header("Attack Cooldown")]
    public float attackCooldown = 1.5f;
    [HideInInspector] public float lastAttackTime = Mathf.NegativeInfinity;

    [Header("Wall Detection")]
    public Transform wallCheck;
    public float wallCheckDistance = 0.2f;
    public LayerMask groundLayer;

    [Header("Ledge Detection")]
    public float ledgeAbortCooldownTime = 3f; // time to delay chasing again after ledge
    [HideInInspector] public float lastTimeLedgeAbort;

    public bool isDead = false;

    public System.Action onFlipped;
    public virtual float stunDuration => 0.5f;

    public virtual EnemyState patrolState => null;
    public virtual EnemyState battleState => null;
    public virtual EnemyState attackState => null;
    public virtual EnemyState stunnedState => null;
    public virtual EnemyState idle => null;
    public Transform groundCheck;
    public float groundCheckDistance = 0.2f;

    public virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        fx = GetComponent<EntityFx>();
        stateMachine = new EnemyStateMachine();
        cd = GetComponent<CapsuleCollider2D>();
        stats = GetComponent<CharacterStats>();
    }

public virtual void Start()
{
    spawnPosition = transform.position;
    initialScale = transform.localScale;
}

public virtual void Update()
{
    if (isDead)
    {
        return; // ❌ Don't call SetZeroVelocity() anymore
    }

    // Player is dead
    if (GameState.isPlayerDead)
    {
        if (stateMachine.currentState != patrolState && patrolState != null)
        {
            stateMachine.ChangeState(patrolState);
        }
    }

    stateMachine?.currentState?.Update();
}




    public void SetVelocity(float x, float y) => rb.velocity = new Vector2(x, y);
    public void SetZeroVelocity() => rb.velocity = Vector2.zero;
    
    private Vector3 initialScale;


public void Flip()
    {
        facingDir *= -1;
        transform.localScale = new Vector3(Mathf.Abs(initialScale.x) * facingDir, initialScale.y, initialScale.z);

        if (onFlipped != null)
            onFlipped();
    }

public void FlipTowardsPlayer()
{
    float previousFacingDir = facingDir;
    if (player == null) return;
    if (isDead) return;

    float dir = player.position.x - transform.position.x;
    facingDir = dir > 0 ? 1 : -1;

    transform.localScale = new Vector3(Mathf.Abs(initialScale.x) * facingDir, initialScale.y, initialScale.z);
    
    if (previousFacingDir != facingDir)
    {
        if (onFlipped != null)
            onFlipped();
    }
}

public virtual bool IsPlayerDetected()
{
    if (GameState.isPlayerDead || player == null)
        return false;

    return Vector2.Distance(transform.position, player.position) <= maxAgroRange;
}

public virtual bool IsPlayerInMinAgroRange()
{
    if (GameState.isPlayerDead || player == null)
        return false;

    return Vector2.Distance(transform.position, player.position) <= minAgroRange;
}

    public virtual bool IsWallDetected()
{
    if (wallCheck == null)
        return false;

    return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, groundLayer);
}

    public virtual bool IsGroundDetected() => true;
    public virtual bool CanBeStunned() => canBeStunned;
    public virtual bool CanAttack() => Time.time >= lastAttackTime + attackCooldown;

public virtual void TakeDamage(bool isPhysical)
{
    if (isDead)
    {
        Debug.Log($"[Enemy] {name} is already dead, ignoring damage.");
        return;
    }

    if (stats == null || PlayerManager.instance?.player?.stats == null)
    {
        Debug.LogError($"[Enemy] Failed to take damage: stats or player is null on {name}");
        return;
    }

    PlayerManager.instance.player.stats.DoDamage(stats, isPhysical);

    if (IsStunned) return;

    //fx?.Flash();

    if (canBeStunned && stunnedState != null)
    {
        // ⛔ Interrupt attack animation if currently attacking
        anim.ResetTrigger("Attack"); // cancel attack anim
        anim.SetTrigger("Stunned");      // play hurt/stun anim

        stateMachine.ChangeState(stunnedState);
    }
}


public virtual void Die()
{
    if (isDead) return;
    isDead = true;

    // ✅ Set velocity to 0 BEFORE making it static
    rb.velocity = Vector2.zero;
    rb.angularVelocity = 0f;

    rb.freezeRotation = true;
    rb.bodyType = RigidbodyType2D.Static; // ✅ Static after velocity is cleared

    if (cd != null)
        cd.enabled = false;

    if (anim != null)
    {
        anim.Rebind();
        anim.Update(0f);

        anim.ResetTrigger("Attack");
        anim.ResetTrigger("Stunned");
        
        anim.SetBool("Stunned", false);
        anim.SetBool("PlayWalk", false);
        anim.SetBool("PlayIdle", false);

        anim.SetTrigger("Die");
    }

    StartCoroutine(FreezeAndDestroy());
}





    private IEnumerator FreezeAndDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    protected virtual void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
if (wallCheck != null)
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance * facingDir);
    }
        Gizmos.color = Color.yellow;
        Vector2 left = Application.isPlaying
            ? spawnPosition - Vector2.right * patrolRange
            : transform.position - Vector3.right * patrolRange;

        Vector2 right = Application.isPlaying
            ? spawnPosition + Vector2.right * patrolRange
            : transform.position + Vector3.right * patrolRange;

        Gizmos.DrawLine(left + Vector2.up * 0.1f, right + Vector2.up * 0.1f);
        Gizmos.DrawSphere(left, 0.1f);
        Gizmos.DrawSphere(right, 0.1f);
#endif
    }

public bool IsGroundAhead()
{
    Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance, Color.red);
    return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
}


    


    public bool IsStunned => stateMachine.currentState == stunnedState;
    public bool IsDead => isDead;

}
