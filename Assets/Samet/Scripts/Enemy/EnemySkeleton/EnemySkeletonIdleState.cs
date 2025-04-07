using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonIdleState : EnemySkeletonGroundedState
{
    public EnemySkeletonIdleState(EnemyStateMachine enemyStateMachine, Enemy enemyBase, string animBoolName, EnemySkeleton enemy) : base(enemyStateMachine, enemyBase, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            enemyStateMachine.ChangeState(enemy.enemySkeletonMoveState);
    }
}
