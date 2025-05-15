using UnityEngine;

public class Medusa : EnemyMelee
{
    public EnemyIdleState idleState { get; private set; }
    public EnemyMoveState moveState { get; private set; }
    public MedusaBattleState battleStateInternal { get; private set; }
    public MedusaAttackState attackStateInternal { get; private set; }
    public EnemyStunnedState stunnedStateInternal { get; private set; } // 🔄 Changed to generic
    public EnemyPatrolState patrolStateInternal { get; private set; }

    public override EnemyState battleState => battleStateInternal;
    public override EnemyState patrolState => patrolStateInternal;
    public override EnemyState attackState => attackStateInternal;
    public override EnemyState stunnedState => stunnedStateInternal;
    public override EnemyState idle => idleState;

    public Transform holdPoint; // 🔹 Assign in Inspector (where the player is pulled into)
    public MedusaSnakeAttackState snakeAttackState { get; private set; }


    public override float stunDuration => 1f;

    public float snakeAttackCooldown = 25f;
    private float lastSnakeTime = Mathf.NegativeInfinity;

    public override void Awake()
    {
        base.Awake();

        idleState = new EnemyIdleState(stateMachine, this, "PlayIdle");
        moveState = new EnemyMoveState(stateMachine, this, "PlayRun");
        battleStateInternal = new MedusaBattleState(stateMachine, this, "PlayRun", this);
        attackStateInternal = new MedusaAttackState(stateMachine, this, "Attack", this);
        stunnedStateInternal = new EnemyStunnedState(stateMachine, this, "Stunned");
        patrolStateInternal = new EnemyPatrolState(stateMachine, this, "PlayWalk");
        snakeAttackState = new MedusaSnakeAttackState(stateMachine, this, "SnakeAttack", this);

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

    public bool CanUseSnakeAttack()
    {
    return Time.time >= lastSnakeTime + snakeAttackCooldown;
    }

    public void UseSnakeAttack()
    {
    if (!CanUseSnakeAttack()) return;

    lastSnakeTime = Time.time;
    stateMachine.ChangeState(snakeAttackState); // ← New custom state for this attack
    }

}
