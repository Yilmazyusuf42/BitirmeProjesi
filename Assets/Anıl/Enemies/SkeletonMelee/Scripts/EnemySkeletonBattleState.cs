using UnityEngine;

public class EnemySkeletonBattleState : EnemyState
{
    private EnemySkeleton enemy;

    public EnemySkeletonBattleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, EnemySkeleton enemy)
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
