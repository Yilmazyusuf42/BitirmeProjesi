using UnityEngine;

public class WerewolfEnemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float chaseRange = 5f;
    public float runAttackRange = 4f;
    public float attackRange = 3f;

    [Header("Attack Settings")]
    public float attackDamage = 10f;
    public float attackCooldown = 0.5f;
    private float lastAttackTime;

    [Header("Health Settings")]
    public float maxHealth = 50f;
    private float currentHealth;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D collider;
    private PlayerHealth playerHealth;
    private bool isDeceased = false;
    private bool isAttacking = false;
    private bool wasRunning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        currentHealth = maxHealth;

        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            Debug.Log("Parameter: " + param.name);
        }
    }

    void Update()
    {
        if (isDeceased || playerHealth.IsDead())
        {
            anim.SetBool("werewolf_run", false);
            rb.velocity = Vector2.zero;
            CancelInvoke();
            isAttacking = false;
            Debug.Log("Stopping due to death");
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Debug.Log($"Distance: {distanceToPlayer:F2} | Chase: {chaseRange} | RunAttack: {runAttackRange} | Attack: {attackRange} | WasRunning: {wasRunning} | IsAttacking: {isAttacking} | TimeSinceAttack: {Time.time - lastAttackTime:F2}");

        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        if (!IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            return;
        }

        AdjustColliderToSprite();

        // Chase until attackRange, with RunAttack as an option
        if (distanceToPlayer > attackRange && distanceToPlayer <= chaseRange && !isAttacking)
        {
            Debug.Log("Chasing");
            ChasePlayer();
            anim.SetBool("werewolf_run", true);
            wasRunning = true;

            // Trigger RunAttack if in range and was running
            if (distanceToPlayer <= runAttackRange && Time.time > lastAttackTime + attackCooldown)
            {
                Debug.Log("Conditions met for RunAttack");
                StartRunAttack();
            }
            anim.Update(0f);
        }
        else if (distanceToPlayer <= attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            Debug.Log("Conditions met for Attack");
            Attack();
        }
        else if (isAttacking && distanceToPlayer > runAttackRange)
        {
            Debug.Log("Player out of range, stopping attack");
            CancelInvoke();
            ResetAttack();
        }
        else if (!isAttacking)
        {
            Debug.Log("Idling");
            anim.SetBool("werewolf_run", false);
            wasRunning = false;
            anim.Update(0f);
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.deltaTime);
    }

    void StartRunAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Debug.Log($"Starting RunAttack - Distance: {distanceToPlayer:F2}");
        if (distanceToPlayer > runAttackRange)
        {
            Debug.Log("RunAttack aborted: Player too far");
            ResetAttack();
            return;
        }

        isAttacking = true;
        anim.SetBool("werewolf_run", false);
        anim.SetTrigger("werewolf_runattack");
        anim.Update(0f);

        lastAttackTime = Time.time;

        if (distanceToPlayer <= runAttackRange)
        {
            Debug.Log("Damaging player from RunAttack");
            playerHealth.TakeDamage(attackDamage);
        }
        Invoke("ChainToAttack", 0.5f); // Match RunAttack duration
    }

    void ChainToAttack()
    {
        if (playerHealth.IsDead())
        {
            ResetAttack();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Debug.Log($"Chaining check - Distance: {distanceToPlayer:F2}");
        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("Chaining to regular attack");
            Attack();
        }
        else
        {
            Debug.Log("Chain skipped: Player out of attack range");
            ResetAttack();
        }
    }

    void Attack()
    {
        if (playerHealth.IsDead()) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        Debug.Log($"Starting Attack - Distance: {distanceToPlayer:F2}");
        if (distanceToPlayer > attackRange)
        {
            Debug.Log("Attack aborted: Player too far");
            ResetAttack();
            return;
        }

        isAttacking = true;
        anim.SetBool("werewolf_run", false);
        int attackChoice = Random.Range(0, 3);
        Debug.Log("Attack Type: " + attackChoice);
        anim.SetFloat("attackType", attackChoice);
        anim.SetTrigger("werewolf_attack");
        anim.Update(0f);

        lastAttackTime = Time.time;

        if (distanceToPlayer <= attackRange)
        {
            Debug.Log("Damaging player");
            playerHealth.TakeDamage(attackDamage);
        }
        Invoke("ResetAttack", 0.7f); // Match longest attack duration
    }

    void ResetAttack()
    {
        Debug.Log("Resetting attack");
        isAttacking = false;
        wasRunning = false;
    }

    public void TakeDamage(float damage)
    {
        if (isDeceased) return;
        currentHealth -= damage;
        anim.SetTrigger("werewolf_hurt");
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        isDeceased = true;
        anim.SetBool("werewolf_dead", true);
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 2f);
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    void AdjustColliderToSprite()
    {
        if (spriteRenderer.sprite == null) return;
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        collider.size = spriteSize;
        collider.offset = new Vector2(0, spriteSize.y / 2f);
    }
}