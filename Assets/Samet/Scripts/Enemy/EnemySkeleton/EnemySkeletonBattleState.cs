using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonBattleState : EnemyState
{
    Transform player;
    EnemySkeleton enemy;
    private int moveDir;
    public EnemySkeletonBattleState(EnemyStateMachine enemyStateMachine, Enemy enemyBase, string animBoolName, EnemySkeleton enemy) : base(enemyStateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            if (enemy.IsPlayerDetected().distance < enemy.attackDist)
            {
                stateTimer = enemy.battleTime;
                if (CanAttack())
                    enemyStateMachine.ChangeState(enemy.enemySkeletonAttackState);
                enemy.SetZeroVelocity();
                return;
            }

        }
        else if (stateTimer < 0 || Vector2.Distance(enemy.transform.position, player.transform.position) > 7)
            enemy.stateMachine.ChangeState(enemy.enemySkeletonIdleState);

        if (enemy.transform.position.x > player.position.x)
            moveDir = -1;
        else if (enemy.transform.position.x < player.position.x)
            moveDir = 1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    private bool CanAttack()
    {
        if (Time.time > enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
            
        return false;
    }
}
