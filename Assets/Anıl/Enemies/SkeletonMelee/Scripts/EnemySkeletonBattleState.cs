using UnityEngine;

public class EnemySkeletonBattleState : EnemyState
{
    private EnemySkeleton enemy;

    public EnemySkeletonBattleState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName, EnemySkeleton enemy)
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
            enemy.anim.SetBool("PlayRun", false); // ✅ idle while cooling

            if (enemy.CanAttack())
            {
                stateMachine.ChangeState(enemy.attackState);
            }
            return;
        }

        if (enemy.IsPlayerDetected())
        {
            // 🛑 Block re-chase if ledge cooldown is active
            if (Time.time < enemy.lastTimeLedgeAbort + enemy.ledgeAbortCooldownTime)
            {
                enemy.SetZeroVelocity();
                enemy.anim.SetBool("PlayRun", false);
                return;
            }

            // ✅ Cooldown passed, move to chase
            stateMachine.ChangeState(enemy.moveState);
        }
        else
        {
            enemy.SetZeroVelocity();
            enemy.anim.SetBool("PlayRun", false);
        }

    }
}
