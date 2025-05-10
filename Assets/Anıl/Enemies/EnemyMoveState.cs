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

        // Chase the player
        enemy.SetVelocity(enemy.runSpeed * enemy.facingDir, rb.velocity.y);

        // If player is within attack range, switch to attack
        if (enemy.IsPlayerInMinAgroRange())
        {
            stateMachine.ChangeState(enemy.attackState);
            return;
        }

        // If player escapes detection, return to patrol
        if (!enemy.IsPlayerDetected())
        {
            stateMachine.ChangeState(enemy.patrolState);
        }
    }
}
