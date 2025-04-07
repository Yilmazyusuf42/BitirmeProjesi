using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    #region States
    public EnemySkeletonMoveState enemySkeletonMoveState { get; private set; }
    public EnemySkeletonIdleState enemySkeletonIdleState { get; private set; }
    public EnemySkeletonBattleState enemySkeletonBattleState { get; private set; }
    public EnemySkeletonAttackState enemySkeletonAttackState { get; private set; }
    public EnemySkeletonStunnedState enemySkeletonStunnedState { get; private set; }
    #endregion
    public override void Awake()
    {
        base.Awake();

        enemySkeletonIdleState = new EnemySkeletonIdleState(stateMachine, this, "Idle", this);
        enemySkeletonMoveState = new EnemySkeletonMoveState(stateMachine, this, "Move", this);
        enemySkeletonBattleState = new EnemySkeletonBattleState(stateMachine,this,"Move",this);
        enemySkeletonAttackState = new EnemySkeletonAttackState(stateMachine, this, "Attack", this);
        enemySkeletonStunnedState = new EnemySkeletonStunnedState(stateMachine, this, "Stunned", this);
    }

    public override void Start()
    {
        base.Start();
        stateMachine.Initialize(enemySkeletonIdleState);
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.U))
            stateMachine.ChangeState(enemySkeletonStunnedState);
    }
    public override bool CanBeStunned()
    {
        if (canBeStunned)
        {
            stateMachine.ChangeState(enemySkeletonStunnedState);
            return true;
        }
        else
            return false;
    }

}
