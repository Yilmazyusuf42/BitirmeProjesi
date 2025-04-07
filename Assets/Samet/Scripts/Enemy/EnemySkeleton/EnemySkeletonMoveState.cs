using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonMoveState : EnemySkeletonGroundedState
{
    public EnemySkeletonMoveState(EnemyStateMachine enemyStateMachine, Enemy enemyBase, string animBoolName, EnemySkeleton enemy) : base(enemyStateMachine, enemyBase, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rb.velocity.y);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();

            enemyStateMachine.ChangeState(enemy.enemySkeletonIdleState);
        }
    }
}
