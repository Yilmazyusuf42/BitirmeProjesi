using UnityEngine;

public class EnemyArcherSkeleton : Enemy
{
    public EnemyIdleState idleState { get; private set; }
    public EnemyPatrolState patrolStateInternal { get; private set; }
    public EnemyArcherCombatState combatState { get; private set; }
    public EnemyArcherAttackState attackStateInternal { get; private set; }
    public EnemyStunnedState stunnedStateInternal { get; private set; }

    public override EnemyState patrolState => patrolStateInternal;
    public override EnemyState battleState => combatState;
    public override EnemyState attackState => attackStateInternal;
    public override EnemyState stunnedState => stunnedStateInternal;
    public override EnemyState idle => idleState;

    [Header("Archer Settings")]
    public float attackCooldown = 2f;
    public float shootRange = 6f;
    public GameObject arrowPrefab;
    public Transform arrowSpawnPoint;

    [HideInInspector] public float lastAttackTime;

    public override void Awake()
    {
        base.Awake();

        idleState = new EnemyIdleState(stateMachine, this, "Idle");
        patrolStateInternal = new EnemyPatrolState(stateMachine, this, "Walk");
        combatState = new EnemyArcherCombatState(stateMachine, this, "Walk", this);
        attackStateInternal = new EnemyArcherAttackState(stateMachine, this, "Attack", this);
        stunnedStateInternal = new EnemyStunnedState(stateMachine, this, "Stunned");
    }

    public override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    // Called by Animation Event
public void ShootArrow()
{
    if (arrowPrefab == null || arrowSpawnPoint == null || player == null)
        return;

    GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);

    if (arrow.TryGetComponent(out Arrow arrowScript))
    {
        Vector2 direction = (player.position - arrowSpawnPoint.position).normalized;
        arrowScript.SetDirection(direction);
    }

    lastAttackTime = Time.time; // ✅ Reset cooldown
}

}
