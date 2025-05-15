using System.Diagnostics;

public class EnemyStunnedState : EnemyState
{
    private float stunDuration;

    public EnemyStunnedState(EnemyStateMachine stateMachine, EnemyBase enemyBase, string animBoolName)
        : base(stateMachine, enemyBase, animBoolName)
    {
        if (enemyBase != null)
        {
            stunDuration = enemyBase.stunDuration;
        }
    }

public override void Enter()
{
    base.Enter();

    // 💀 If enemy is already dead, don't enter stun state
    if (enemyBase.stats.currentHp <= 0 || enemyBase.IsDead)
    {
        enemyBase.Die(); // just in case it hasn’t been called yet
        return;
    }

    enemyBase.SetZeroVelocity();
    stateTimer = enemyBase.stunDuration;

    enemyBase.anim.SetBool("Stunned", true);
}


public override void Exit()
{
    base.Exit();

    // Only clear animation if not dead
    if (!enemyBase.IsDead)
    {
        enemyBase.anim.SetBool("Stunned", false);
    }

    enemyBase.canBeStunned = false;
}


public override void Update()
{
    base.Update();

    if (enemyBase.IsDead)
        return; // stop state logic if dead

    if (stateTimer <= 0)
    {
        stateMachine.ChangeState(enemyBase.idle);
    }
}

}
