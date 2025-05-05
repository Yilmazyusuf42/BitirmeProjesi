using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonStunnedState : EnemyState
{
    EnemySkeleton enemy;
    public EnemySkeletonStunnedState(EnemyStateMachine enemyStateMachine, Enemy enemyBase, string animBoolName, EnemySkeleton enemy) : base(enemyStateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.stunnedDuration;

        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunnedDirection.x, enemy.stunnedDirection.y);

        enemy.entityFx.InvokeRepeating("RedColorBlink", 0, 0.1f);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.entityFx.Invoke("CancelRedBlink",0);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            enemyStateMachine.ChangeState(enemy.enemySkeletonIdleState);
    }
}
