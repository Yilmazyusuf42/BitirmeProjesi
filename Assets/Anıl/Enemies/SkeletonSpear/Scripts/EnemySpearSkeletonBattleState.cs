using UnityEngine;

public class EnemySpearSkeletonBattleState : EnemyState
{
    private EnemySpearSkeleton enemy;

    public EnemySpearSkeletonBattleState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName, EnemySpearSkeleton enemy)
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

    float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.player.position);

    if (distanceToPlayer <= enemy.minAgroRange)
    {
        enemy.SetZeroVelocity();
        enemy.anim.SetBool("PlayRun", false); // ✅ stop running animation

        if (enemy.CanAttack())
        {
            stateMachine.ChangeState(enemy.attackState);
        }

        return;
    }

    // Player is still detected and we’re not in attack range
    if (enemy.IsPlayerDetected())
    {
        enemy.SetVelocity(enemy.runSpeed * enemy.facingDir, rb.velocity.y);
        enemy.anim.SetBool("PlayRun", true); // ✅ chase animation
    }
    else
    {
        stateMachine.ChangeState(enemy.moveState);
    }
}

}
