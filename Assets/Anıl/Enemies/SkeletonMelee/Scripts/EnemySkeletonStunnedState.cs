public class EnemySkeletonStunnedState : EnemyState
{
    private EnemySkeleton enemy;

    public EnemySkeletonStunnedState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, EnemySkeleton enemy)
        : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
