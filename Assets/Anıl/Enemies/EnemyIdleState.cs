using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName)
        : base(stateMachine, enemyBase, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemyBase.idleTime;
        
    }

    public override void Update()
    {
        base.Update();

        if (enemyBase.IsPlayerDetected())
        {
            if (enemyBase.battleState != null && stateMachine != null)
            {
                stateMachine.ChangeState(enemyBase.battleState);
            }
            return;
        }

        if (stateTimer <= 0)
        {
            if (enemyBase.patrolState != null && stateMachine != null)
            {
                stateMachine.ChangeState(enemyBase.patrolState);
            }
        }
    }
}
