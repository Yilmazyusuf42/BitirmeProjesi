using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Stunned info")]
    public float stunnedDuration;
    public Vector2 stunnedDirection;
    [SerializeField] protected GameObject counterImage;
    protected bool canBeStunned;
    

    [Header("Attack Info")]
    public float attackDist;
    public float attackCooldown;
    public float battleTime;
   [HideInInspector] public float lastTimeAttacked;

    [Header("Move Info")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float idleTime;

    [Header("Coliision Info")]
    [SerializeField] protected LayerMask whatIsPlayer;
    public EnemyStateMachine stateMachine { get; private set; }

    public override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }
    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        base.Update();
        
        stateMachine.currentState.Update();
    }
    public virtual void OpenAttackCounterWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }
    public virtual void CloseAttackCounterWindow()
    {
        canBeStunned = false; 
        counterImage?.SetActive(false);
    }
    public virtual bool CanBeStunned()
    {
        if(canBeStunned)
            return true;
        return false;
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
    public RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.transform.position, Vector2.right*facingDir, 50, whatIsPlayer);
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(wallCheck.transform.position, new Vector3(wallCheck.position.x + attackDist*facingDir, wallCheck.position.y));
    }
}
