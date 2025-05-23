using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFx entityFx { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [Header("Attack info")]
    public Transform attackCheck;
    public float attackRadius;

    [Header("KnockBack Info")]
    [SerializeField] protected Vector2 knockBackDirection;
    protected bool isKnocked;
    public float     knockBackTime;

    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;


    public System.Action onFlipped;
    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public virtual void Awake()
    {
    
    }
    public virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        entityFx = GetComponent<EntityFx>();
        stats = GetComponent<CharacterStats>();
        cd= GetComponent<CapsuleCollider2D>();
    }
    public virtual void Update()
    {
       
    }
    public void Damage()
    {
        entityFx.StartCoroutine("FlashFx");
        StartCoroutine("HitKnockback");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockBackDirection.x * -facingDir , knockBackDirection.y);

        yield return new WaitForSeconds(knockBackTime);

        isKnocked = false;
    }
    #region Velocity
    public virtual void SetZeroVelocity()
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(0, 0);
    }

    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    #endregion
    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance*facingDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
    #endregion
    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if(onFlipped!=null)
        onFlipped();
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }

    #endregion
}
