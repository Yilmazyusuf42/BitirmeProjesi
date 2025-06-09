using UnityEngine;

public class EnemyMoveState : EnemyState
{
    private EnemyBase enemy;

    public EnemyMoveState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName)
        : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemyBase;
    }

    public override void Enter()
    {
        base.Enter();

        // Flip towards player for chasing
        enemy.FlipTowardsPlayer();

        // Start run animation
        enemy.anim.SetBool("PlayRun", true);
    }

    public override void Exit()
    {
        base.Exit();

        // Stop run animation
        enemy.anim.SetBool("PlayRun", false);
    }

public override void Update()
{
    base.Update();

    // 🛑 Stop and do nothing if there's no ground ahead
    if (!enemy.IsGroundAhead())
    {
        enemy.SetZeroVelocity();
        enemy.lastTimeLedgeAbort = Time.time; // ✅ mark ledge fallback
        stateMachine.ChangeState(enemy.patrolState); // or patrolState

        return;
    }

    // ✅ Chase player if safe
    enemy.SetVelocity(enemy.runSpeed * enemy.facingDir, rb.velocity.y);

    // 🎯 Attack if close enough
    if (enemy.IsPlayerInMinAgroRange())
    {
        stateMachine.ChangeState(enemy.attackState);
        return;
    }

    // 👀 If player escapes, return to patrol
    if (!enemy.IsPlayerDetected())
    {
        stateMachine.ChangeState(enemy.patrolState);
    }
}


}
