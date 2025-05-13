using UnityEngine;

public class EnemyWerewolf : EnemyMelee
{
    public EnemyIdleState idleState { get; private set; }
    public EnemyMoveState moveState { get; private set; }
    public EnemyWerewolfBattleState battleStateInternal { get; private set; }
    public EnemyWerewolfAttackState attackStateInternal { get; private set; }
    public EnemyStunnedState stunnedStateInternal { get; private set; } // 🔄 Changed to generic
    public EnemyPatrolState patrolStateInternal { get; private set; }

    public override EnemyState battleState => battleStateInternal;
    public override EnemyState patrolState => patrolStateInternal;
    public override EnemyState attackState => attackStateInternal;
    public override EnemyState stunnedState => stunnedStateInternal;
    public override EnemyState idle => idleState;

    public override float stunDuration => 1f;

    // Assign all states in Awake
    public override void Awake()
    {
        base.Awake();

        idleState = new EnemyIdleState(stateMachine, this, "PlayIdle");
        moveState = new EnemyMoveState(stateMachine, this, "PlayRun");
        battleStateInternal = new EnemyWerewolfBattleState(stateMachine, this, "PlayRun", this);
        attackStateInternal = new EnemyWerewolfAttackState(stateMachine, this, "Attack", this);
        stunnedStateInternal = new EnemyStunnedState(stateMachine, this, "Stunned");
        patrolStateInternal = new EnemyPatrolState(stateMachine, this, "PlayWalk");

      //  Debug.Log($"[EnemyWerewolf] Awake for {gameObject.name}. patrolStateInternal: {(patrolStateInternal != null ? "Initialized" : "NULL")}, battleStateInternal: {(battleStateInternal != null ? "Initialized" : "NULL")}");
    }

    // Initialize FSM in Start
    public override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        spawnPosition = transform.position;

    }

    // Hook stun logic
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
