using UnityEngine;

public class EnemySpearSkeletonBattleState : EnemyState
{
    private EnemySpearSkeleton enemy;

    public EnemySpearSkeletonBattleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, EnemySpearSkeleton enemy)
        : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool("PlayRun", true);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.anim.SetBool("PlayRun", false);
    }

    public override void Update()
    {
        base.Update();

        enemy.FlipTowardsPlayer();
        enemy.SetVelocity(enemy.runSpeed * enemy.facingDir, rb.velocity.y);

        if (enemy.IsPlayerInMinAgroRange())
        {
            stateMachine.ChangeState(enemy.attackState);
            return;
        }

        if (!enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.moveState);
            return;
        }
    }
}
