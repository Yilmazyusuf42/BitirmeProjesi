using UnityEngine;

public class EnemyFireWizardBattleState : EnemyState
{
    private EnemyFireWizard enemy;

    public EnemyFireWizardBattleState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName, EnemyFireWizard enemy)
        : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool("PlayIdle", true); // optionally use walk/run animation
    }

    public override void Exit()
    {
        base.Exit();
        enemy.anim.SetBool("PlayIdle", false);
    }

public override void Update()
{
    base.Update();

    enemy.FlipTowardsPlayer();

    float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.player.position);

    if (distanceToPlayer <= enemy.minAgroRange)
    {
        enemy.SetZeroVelocity();
        enemy.anim.SetBool("PlayIdle", false);

        if (enemy.CanAttack())
        {
            stateMachine.ChangeState(enemy.attackState);
        }

        return;
    }

    // If player is within detection range but outside melee range
    if (enemy.IsPlayerDetected())
    {
        enemy.SetZeroVelocity();
        if (enemy.CanUseMagic())
        {
            stateMachine.ChangeState(enemy.castState);
        }
    }
    else
    {
        stateMachine.ChangeState(enemy.patrolState);
    }
}

}
