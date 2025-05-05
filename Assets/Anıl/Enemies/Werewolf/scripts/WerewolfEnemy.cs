using UnityEngine;

public class WerewolfEnemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float chaseRange = 5f;
    public float runAttackRange = 4f;
    public float attackRange = 3f;

    public float attackDamage = 10f;
    public float attackCooldown = 0.5f;
    private float lastAttackTime;

    public float maxHealth = 50f;
    private float currentHealth;

    private readonly float attack0Length = 0.5f; // Set manually for werewolf_attack_0
    private readonly float attack1Length = 0.333f; // Set manually for werewolf_attack_1
    private readonly float attack2Length = 0.417f; // Set manually for werewolf_attack_2
    private readonly float runAttackLength = 0.583f; // Set manually for werewolf_runattack

    private bool hasDoneRunAttack = false;

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
        anim.SetBool("isAttacking", false);
    }

    void Update()
    {
        if (isDeceased || playerHealth.IsDead())
        {
            anim.SetBool("werewolf_run", false);
            rb.velocity = Vector2.zero;
            CancelInvoke();
            isAttacking = false;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

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

        if (distanceToPlayer <= attackRange && Time.time > lastAttackTime + attackCooldown && !isAttacking)
        {
            Attack();
        }
        else if (distanceToPlayer > attackRange && distanceToPlayer <= chaseRange && !isAttacking)
        {
            ChasePlayer();
            anim.SetBool("werewolf_run", true);
            wasRunning = true;

            if (distanceToPlayer <= runAttackRange && Time.time > lastAttackTime + attackCooldown && wasRunning && !hasDoneRunAttack)
            {
                StartRunAttack();
            }
        }
        else if (isAttacking && distanceToPlayer > runAttackRange)
        {
            CancelInvoke();
            ResetAttack();
        }
        else if (!isAttacking)
        {
            if (distanceToPlayer > runAttackRange)
                hasDoneRunAttack = false;
            anim.SetBool("werewolf_run", false);
            wasRunning = false;
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
        if (distanceToPlayer > runAttackRange)
        {
            ResetAttack();
            return;
        }

        isAttacking = true;
        hasDoneRunAttack = true;
        anim.SetBool("werewolf_run", false);
        anim.SetTrigger("werewolf_runattack");
        anim.SetBool("isAttacking", true);
        anim.Update(0f);

        lastAttackTime = Time.time;

        if (distanceToPlayer <= runAttackRange)
        {
            Invoke("ApplyRunAttackDamage", runAttackLength * 0.5f);
        }
        Invoke("ChainToAttack", runAttackLength);
    }

    void ChainToAttack()
    {
        if (playerHealth.IsDead())
        {
            ResetAttack();
            return;
        }

        Attack();
    }

    void Attack()
    {
        if (playerHealth.IsDead()) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > attackRange)
        {
            ResetAttack();
            return;
        }

        isAttacking = true;
        anim.SetBool("werewolf_run", false);
        int attackChoice = Random.Range(0, 3);
        anim.SetFloat("attackType", attackChoice);
        anim.SetTrigger("werewolf_attack");
        anim.SetBool("isAttacking", true);
        anim.Update(0f);

        lastAttackTime = Time.time;

        float damageDelay = GetAttackAnimationLength(attackChoice) * 0.5f;
        Invoke("ApplyAttackDamage", damageDelay);
        Invoke("ResetAttack", GetAttackAnimationLength(attackChoice));
    }

    float GetAttackAnimationLength(int attackChoice)
    {
        switch (attackChoice)
        {
            case 0: return attack0Length;
            case 1: return attack1Length;
            case 2: return attack2Length;
            default: return 0.7f;
        }
    }

    void ApplyRunAttackDamage()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= runAttackRange + 0.5f && !playerHealth.IsDead())
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    void ApplyAttackDamage()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange + 0.5f && !playerHealth.IsDead())
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
        wasRunning = false;
        anim.ResetTrigger("werewolf_runattack");
        anim.ResetTrigger("werewolf_attack");
        anim.SetBool("isAttacking", false);
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
        collider.enabled = false;
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