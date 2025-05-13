using UnityEngine;

public class EnemySkeleton : EnemyMelee
{
    public EnemyIdleState idleState { get; private set; }
    public EnemyMoveState moveState { get; private set; }
    public EnemySkeletonBattleState battleStateInternal { get; private set; }
    public EnemySkeletonAttackState attackStateInternal { get; private set; }
    public EnemyStunnedState stunnedStateInternal { get; private set; } // 🔄 Changed to generic
    public EnemyPatrolState patrolStateInternal { get; private set; }

    public override EnemyState battleState => battleStateInternal;
    public override EnemyState patrolState => patrolStateInternal;
    public override EnemyState attackState => attackStateInternal;
    public override EnemyState stunnedState => stunnedStateInternal;
    public override EnemyState idle => idleState;

    public override float stunDuration => 0.3f;
    
    [HideInInspector]
    public int lastAttackIndex = -1;


    public override void Awake()
    {
        base.Awake();

        idleState = new EnemyIdleState(stateMachine, this, "PlayIdle");
        moveState = new EnemyMoveState(stateMachine, this, "PlayRun");
        battleStateInternal = new EnemySkeletonBattleState(stateMachine, this, "PlayRun", this);
        attackStateInternal = new EnemySkeletonAttackState(stateMachine, this, "Attack", this);
        stunnedStateInternal = new EnemyStunnedState(stateMachine, this, "Stunned");
        patrolStateInternal = new EnemyPatrolState(stateMachine, this, "PlayWalk");
    }

    public override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        spawnPosition = transform.position;
    }

    public override bool CanBeStunned()
    {
        if (canBeStunned)
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }
}
