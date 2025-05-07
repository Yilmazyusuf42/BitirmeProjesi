using UnityEngine;

public class EnemyWerewolf : Enemy
{
    // State references
    public EnemyIdleState idleState { get; private set; }
    public EnemyMoveState moveState { get; private set; }
    public EnemyWerewolfBattleState battleStateInternal { get; private set; }
    public EnemyWerewolfAttackState attackStateInternal { get; private set; }
    public EnemyWerewolfStunnedState stunnedState { get; private set; }
    public EnemyPatrolState patrolStateInternal { get; private set; }

    public override EnemyState battleState => battleStateInternal;
    public override EnemyState patrolState => patrolStateInternal;
    public override EnemyState attackState => attackStateInternal;

    // Assign all states in Awake
    public override void Awake()
    {
        base.Awake();

        idleState = new EnemyIdleState(stateMachine, this, "Idle");
        moveState = new EnemyMoveState(stateMachine, this, "Run");
        battleStateInternal = new EnemyWerewolfBattleState(stateMachine, this, "Battle", this);
        attackStateInternal = new EnemyWerewolfAttackState(stateMachine, this, "Attack", this);
        stunnedState = new EnemyWerewolfStunnedState(stateMachine, this, "Stunned", this);
        patrolStateInternal = new EnemyPatrolState(stateMachine, this, "Walk");

        Debug.Log($"[EnemyWerewolf] Awake for {gameObject.name}. patrolStateInternal: {(patrolStateInternal != null ? "Initialized" : "NULL")}, battleStateInternal: {(battleStateInternal != null ? "Initialized" : "NULL")}");
    }

    // Initialize FSM in Start
    public override void Start()
    {
        base.Start();
        Debug.Log("[Werewolf] patrolState null? " + (patrolState == null));
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
