using UnityEngine;

public class EnemyArcherBattleState : EnemyState
{
    private EnemyArcherSkeleton enemy;

    public EnemyArcherBattleState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName, EnemyArcherSkeleton enemy)
        : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Update()
    {
        base.Update();

        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);
        enemy.FlipTowardsPlayer();

        if (distance <= enemy.minAgroRange)
        {
            // Too close, move away
            enemy.SetVelocity(-enemy.runSpeed * enemy.facingDir, enemy.rb.velocity.y);
        }
        else if (distance <= enemy.shootRange)
        {
            enemy.SetZeroVelocity();

            if (Time.time >= enemy.lastAttackTime + enemy.attackCooldown)
            {
                stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            stateMachine.ChangeState(enemy.patrolState);
        }
    }
}
