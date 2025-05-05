using UnityEngine;

public class SkeletonMelee : MonoBehaviour
{
    [Header("References")]
    private Transform player;
    private Animator anim;
    private Rigidbody2D rb;

    [Header("Movement")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;
    public float patrolRange = 3f;
    private Vector2 spawnPoint;
    private bool movingRight = true;

    [Header("Detection")]
    public float chaseRange = 6f;
    private bool playerDetected;
    private bool isAttacking;
    private bool returningToSpawn;

    [Header("Attack")]
    public Transform attackPoint;
    public float attackRadius = 0.5f;
    public LayerMask playerLayer;
    public float attackCooldown = 2f;
    private float lastAttackTime;
    private float attackTimeout = 1.5f;
    private float attackTimer;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spawnPoint = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log("Skeleton spawned at " + spawnPoint);
        currentHealth = maxHealth;

    }

    private void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        Debug.Log("Distance to player: " + distToPlayer);

        if (isAttacking)
{
    // let the attack animation finish
    attackTimer -= Time.deltaTime;
    if (attackTimer <= 0)
    {
        Debug.LogWarning("Forced attack reset — animation event failed?");
        isAttacking = false;
    }
    return;
}


        anim.SetBool("smelee_idle", false);
        anim.SetBool("smelee_run", false);
        anim.SetBool("smelee_walk", false);

        if (IsPlayerInAttackZone())
        {
            Debug.Log("✅ Player in attack zone.");
            rb.velocity = Vector2.zero;
            anim.SetBool("smelee_idle", true);
            StartAttack();
        }
        else if (distToPlayer <= chaseRange)
        {
            Debug.Log("👁️ Chasing player.");
            ChasePlayer();
            anim.SetBool("smelee_run", true);
        }
        else if (playerDetected)
        {
            Debug.Log("🔙 Returning to spawn.");
            ReturnToSpawn();
            anim.SetBool("smelee_run", true);
        }
        else
        {
            Debug.Log("👣 Patrolling.");
            Patrol();
            anim.SetBool("smelee_walk", true);
        }

        FlipDirectionIfNeeded();
    }

    private void Patrol()
    {
        playerDetected = false;
        returningToSpawn = false;
        rb.velocity = new Vector2((movingRight ? 1 : -1) * walkSpeed, rb.velocity.y);

        if (movingRight && transform.position.x >= spawnPoint.x + patrolRange)
{
    movingRight = false;
    Debug.Log("Flipped left at patrol edge.");
}
else if (!movingRight && transform.position.x <= spawnPoint.x - patrolRange)
{
    movingRight = true;
    Debug.Log("Flipped right at patrol edge.");
}

    }

    private void ChasePlayer()
    {
        playerDetected = true;
        returningToSpawn = false;

        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * runSpeed, rb.velocity.y);
        Debug.Log("🏃‍♂️ Running toward player. Direction: " + direction);
    }

    private void ReturnToSpawn()
    {
        returningToSpawn = true;

        float direction = Mathf.Sign(spawnPoint.x - transform.position.x);
        rb.velocity = new Vector2(direction * walkSpeed, rb.velocity.y);
        Debug.Log("🏠 Returning to spawn. Direction: " + direction);

        if (Mathf.Abs(transform.position.x - spawnPoint.x) <= 0.1f)
        {
            returningToSpawn = false;
            playerDetected = false;
            Debug.Log("✅ Returned to spawn.");
        }
    }

    private void StartAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
        {
            Debug.Log("⏳ Skeleton: attack on cooldown.");
            return;
        }

        attackTimer = attackTimeout;
        isAttacking = true;
        lastAttackTime = Time.time;

        float attackIndex = Mathf.Floor(Random.Range(1f, 4f));
        Debug.Log($"🗡️ Skeleton: ATTACKING — attackType = {attackIndex}");

        anim.ResetTrigger("smelee_attack");
        anim.SetTrigger("smelee_attack");
        anim.SetFloat("attackType", attackIndex);
    }

    public void EndAttack()
    {
        Debug.Log("✅ Attack finished → Skeleton ready to attack again.");
        isAttacking = false;
    }

    public void DamagePlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);

        foreach (var hit in hits)
        {
            if (hit.GetComponent<Player>() != null)
            {
                Debug.Log("💥 Skeleton hit the player!");
                // hit.GetComponent<Player>().TakeDamage(damageAmount);
            }
        }
    }

    public void Damage()
    {
        anim.SetTrigger("smelee_hurt");
        Debug.Log("😵 Skeleton took damage.");
    }

    public void TakeDamage(int amount)
{
    if (isDead) return;

    currentHealth -= amount;
    Debug.Log($"💢 Skeleton took {amount} damage. Remaining HP: {currentHealth}");

    if (currentHealth > 0)
    {
        anim.SetTrigger("smelee_hurt");
    }
    else
    {
        Die();
    }
}

private void Die()
{
    Debug.Log("💀 Skeleton died.");
    isDead = true;
    rb.velocity = Vector2.zero;

    anim.SetTrigger("smelee_dead");

    // Optional: disable logic after death
    this.enabled = false; // or disable collider, AI, etc.
}


    public bool CanBeStunned()
    {
        return true;
    }

    private void FlipDirectionIfNeeded()
    {
        if (rb.velocity.x > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (rb.velocity.x < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private bool IsPlayerInAttackZone()
    {
        bool inZone = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);
        if (inZone)
        {
            Debug.Log("🎯 Player is inside the attack radius.");
        }
        return inZone;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}
