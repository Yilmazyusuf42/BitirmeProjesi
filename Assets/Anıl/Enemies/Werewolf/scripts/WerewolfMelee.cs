using UnityEngine;

public class WerewolfMelee : MonoBehaviour
{
    [Header("References")]
    private Transform player;
    private Animator anim;
    private Rigidbody2D rb;

    [Header("Movement")]
    public float walkSpeed = 1.8f;
    public float runSpeed = 4f;
    public float patrolRange = 4f;
    private Vector2 spawnPoint;
    private bool movingRight = true;

    [Header("Detection")]
    public float chaseRange = 7f;
    private bool playerDetected;
    private bool isAttacking;
    private bool returningToSpawn;

    [Header("Attack")]
    public Transform attackPoint;
    public float attackRadius = 0.6f;
    public LayerMask playerLayer;
    public float attackCooldown = 2f;
    private float lastAttackTime;
    private float attackTimeout = 1.5f;
    private float attackTimer;

    [Header("Health")]
    public int maxHealth = 5;
    private int currentHealth;
    private bool isDead;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spawnPoint = transform.position;
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    private void Update()
    {
        if (isDead) return;

        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0)
            {
                //Debug.logWarning("Werewolf attack timeout — reset.");
                isAttacking = false;
            }
            return;
        }

        anim.SetBool("werewolf_idle", false);
        anim.SetBool("werewolf_run", false);
        anim.SetBool("werewolf_walk", false);

        if (IsPlayerInAttackZone())
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("werewolf_idle", true);
            StartAttack();
        }
        else if (distToPlayer <= chaseRange)
        {
            ChasePlayer();
            anim.SetBool("werewolf_run", true);
        }
        else if (playerDetected)
        {
            ReturnToSpawn();
            anim.SetBool("werewolf_run", true);
        }
        else
        {
            Patrol();
            anim.SetBool("werewolf_walk", true);
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
        }
        else if (!movingRight && transform.position.x <= spawnPoint.x - patrolRange)
        {
            movingRight = true;
        }
    }

    private void ChasePlayer()
    {
        playerDetected = true;
        returningToSpawn = false;

        float direction = Mathf.Sign(player.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * runSpeed, rb.velocity.y);
    }

    private void ReturnToSpawn()
    {
        returningToSpawn = true;

        float direction = Mathf.Sign(spawnPoint.x - transform.position.x);
        rb.velocity = new Vector2(direction * walkSpeed, rb.velocity.y);

        if (Mathf.Abs(transform.position.x - spawnPoint.x) <= 0.1f)
        {
            returningToSpawn = false;
            playerDetected = false;
        }
    }

    private void StartAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        isAttacking = true;
        lastAttackTime = Time.time;
        attackTimer = attackTimeout;

        float attackIndex = Mathf.Round(Random.Range(1f, 3.99f));
        anim.ResetTrigger("werewolf_attack");
        anim.SetTrigger("werewolf_attack");
        anim.SetFloat("attackType", attackIndex);
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    public void DamagePlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);
        foreach (var hit in hits)
        {
            if (hit.GetComponent<Player>() != null)
            {
                //Debug.log("Werewolf hit the player!");
                // hit.GetComponent<Player>().TakeDamage(damageAmount);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth > 0)
        {
            anim.SetTrigger("werewolf_hurt");
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        anim.SetTrigger("werewolf_dead");
        this.enabled = false;
    }

    private bool IsPlayerInAttackZone()
    {
        return Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);
    }

    private void FlipDirectionIfNeeded()
    {
        if (rb.velocity.x > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (rb.velocity.x < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
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
