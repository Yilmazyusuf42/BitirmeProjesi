using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12;
    private Animator anim;
    private Rigidbody2D rb;
    private CapsuleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;
    private Transform sword;

    [Header("Pierce Info")]
    [SerializeField] private int amountOfPierce;


    [Header("Bounce info")]
    [SerializeField] private float bounceSpeed;
    private bool isBouncing;
    private int amountOfBounce;
    private List<Transform> enemyTargets;
    private int targetIndex;

    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;


    private float hitTimer;
    private float hitCooldown;

    private float spinDirection;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity.normalized;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.ClearSword();
                sword = player.sword.transform;

                if (player.transform.position.x > sword.transform.position.x && player.facingDir == 1)
                    player.Flip();
                else if (player.transform.position.x < sword.transform.position.x && player.facingDir == -1)
                    player.Flip();
            }
        }
        BounceLogic();

        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<EnemyBase>() != null)
                            hit.GetComponent<EnemyBase>().TakeDamage(true);

                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0 && enemyTargets[targetIndex]!=null)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < .1f)
            {
                targetIndex++;
                amountOfBounce--;
                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (targetIndex >= enemyTargets.Count)
                    targetIndex = 0;
            }
        }
    }

    public void SetSword(Vector2 _dir, float _gravityScale, Player _player,float _returnSpeed)
    {
        player = _player;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        returnSpeed= _returnSpeed;
        anim.SetBool("Rotate", true);


        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBounces,float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounces;
        bounceSpeed = _bounceSpeed;
        enemyTargets = new List<Transform>();
    }

    public void SetupPierce(int _amountOfPierce)
    {
        amountOfPierce = _amountOfPierce;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetBool("Rotate", true);
        transform.parent = null;
        isReturning = true;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        collision.GetComponent<EnemyBase>()?.TakeDamage(true);

        SetupTargetsForBounce(collision);
        StukInto(collision);
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<EnemyBase>()!=null)
        {
            if (isBouncing && enemyTargets.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<EnemyBase>() != null)
                        enemyTargets.Add(hit.transform);
                }
            }
        }
    }

    private void StukInto(Collider2D collision)
    {
        if (amountOfPierce > 0 && collision.tag == "Enemy")
        {
            amountOfPierce--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }


        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTargets.Count > 0)
            return;

        anim.SetBool("Rotate", false);
        transform.parent = collision.transform;
    }
}
