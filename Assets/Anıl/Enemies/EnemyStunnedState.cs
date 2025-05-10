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

        enemyBase.SetZeroVelocity();
        stateTimer = enemyBase.stunDuration;
        // Play the hurt animation
        enemyBase.anim.SetBool("Stunned",true);
    }

    public override void Exit()
    {
        base.Exit();
        enemyBase.anim.SetBool("Stunned", false);
        enemyBase.canBeStunned = false;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer <= 0)
        {
            // Transition to the shared idle state property
            stateMachine.ChangeState(enemyBase.idle);
        }
    }
}
