using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAttackState : EnemyState
{
    EnemySkeleton enemy;
    public EnemySkeletonAttackState(EnemyStateMachine enemyStateMachine, Enemy enemyBase, string animBoolName, EnemySkeleton enemy) : base(enemyStateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            enemyStateMachine.ChangeState(enemy.enemySkeletonBattleState);
        }
    }
}
