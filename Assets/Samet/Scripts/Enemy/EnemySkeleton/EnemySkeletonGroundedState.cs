using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonGroundedState : EnemyState
{
    Transform player;
    protected EnemySkeleton enemy;
    public EnemySkeletonGroundedState(EnemyStateMachine enemyStateMachine, Enemy enemyBase, string animBoolName, EnemySkeleton enemy) : base(enemyStateMachine, enemyBase, animBoolName)
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

        if (enemy.IsPlayerDetected()|| Vector2.Distance(enemy.transform.position,player.transform.position)<2)
           enemyStateMachine.ChangeState(enemy.enemySkeletonBattleState);
    }
}
